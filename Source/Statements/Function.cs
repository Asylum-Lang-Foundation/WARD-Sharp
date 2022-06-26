using LLVMSharp.Interop;
using WARD.Common;
using WARD.Exceptions;
using WARD.Scoping;
using WARD.Types;

namespace WARD.Statements;

// Create a function. TODO: FUNCTION FOR DECLARING DEFINTION!!!
public class Function : Variable, ICompileableTopLevel {
    private LLVMValueRef CompiledVal = null; // If function has already been compiled.
    public Scope Scope { get; internal set; } // Scope for items within the function.
    public string FuncName { get; } // Actual name of the function, Name is reserved for the mangled version.
    public ItemAttribute[] Attributes { get; } // Function attributes for additional rules for handling.
    public bool Inline => Attributes.Where(x => x.Name.Equals("Inline")).Count() > 0; // If the function serves as a macro rather than being defined.
    public CodeStatements Definition { get; internal set; } = null; // Function definition if one exists.
    public FileContext FileContext { get; } // File context of where the function is.
    public bool NameMangled => FuncName != "main" && Attributes.Where(x => x.Name.Equals("NoMangle")).Count() < 1; // If the function's name is mangled.
    public override string ToString() => NameMangled ? (Name.Length + Name + Type.Mangled()) : Name;
    public FileContext GetFileContext() => FileContext;

    // Create a new function. WARNING: This does not add the function to the scope (do it manually to add both the mangled variable name and regular function name).
    public Function(string name, VarTypeFunction signature, params ItemAttribute[] attributes) : base(name.Length + name + signature.Mangled(), signature, DataAccessFlags.Read | DataAccessFlags.Static) {
        FuncName = name;
        Attributes = attributes;
        if (!NameMangled) Name = name;
    }

    public void SetScopes(Scope parent) {
        if (Definition != null) {
            Scope = parent.EnterScope(Name, false);
            Definition.SetScopes(Scope);
        }
    }

    public void ResolveVariables() {
        // Shadow parameters.
        foreach (var v in (Type as VarTypeFunction).Parameters) {
            var newVar = new Variable(v.Name, v.Type, v.AccessFlags);
            Scope.Table.AddVariable(newVar);
        }
        if (Definition != null) Definition.ResolveVariables();
    }

    public void ResolveTypes() {
        if (Definition != null) Definition.ResolveTypes();
    }

    public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder) {} // Nothing to do here as we are calling this in the compile function.

    public LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {

        // Do no work if already compiled or not able to be compiled.
        if (Inline || CompiledVal != null) return CompiledVal;

        // Initialize compiled value.
        CompiledVal = mod.AddFunction(ToString(), Type.GetLLVMType());

        // Do not continue if there is no defintion.
        if (Definition == null) return CompiledVal;

        // Add block for building.
        var block = LLVMBasicBlockRef.AppendInContext(mod.Context, CompiledVal, "entry");
        builder.PositionAtEnd(block);

        // Shadow parameters.
        int i = 0;
        foreach (var v in (Type as VarTypeFunction).Parameters) {
            Variable resolved = Scope.Table.ResolveVariable(v.Name);
            resolved.Value = builder.BuildAlloca(
                v.Type.GetLLVMType(),
                "W_Param_" + i
            );
            builder.BuildStore(CompiledVal.Params[i++], resolved.Value);
        }

        // Compile function.
        Definition.CompileDeclarations(mod, builder);
        Definition.Compile(mod, builder, param);

        // Automatically insert return if none detected.
        if (!param.LastBlock.BlockTerminated) {
            if ((Type as VarTypeFunction).ReturnType.Equals(VarType.Void)) {
                param.LastBlock.ReturnAValue(builder.BuildRetVoid());
            } else {
                Error.ThrowInternal(Name + " does not return a value.");
                return null;
            }
        }

        // Finished compiling, verify and optimize.
        CompiledVal.VerifyFunction(LLVMVerifierFailureAction.LLVMPrintMessageAction);
        param.Fpm.RunFunctionPassManager(CompiledVal);
        return CompiledVal;

    }

}