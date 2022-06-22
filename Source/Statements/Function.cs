using LLVMSharp.Interop;
using WARD.Common;
using WARD.Exceptions;
using WARD.Scoping;
using WARD.Types;

namespace WARD.Statements;

// Create a function. TODO: FUNCTION FOR DECLARING DEFINTION!!!
public class Function : Variable, ICompileableTopLevel {
    public Scope Scope { get; internal set; } // Scope for items within the function.
    public string FuncName { get; } // Actual name of the function, Name is reserved for the mangled version.
    public ItemAttribute[] Attributes { get; } // Function attributes for additional rules for handling.
    public bool Inline { get; } = false; // If the function serves as a macro rather than being defined.
    public CodeStatements Definition { get; internal set; } = null; // Function definition if one exists.
    public FileContext FileContext { get; } // File context of where the function is.
    public bool NameMangled => FuncName != "main" && Attributes.Where(x => x.Name.Equals("NoMangle")).Count() < 1; // If the function's name is mangled.
    public FileContext GetFileContext() => FileContext;

    // Create a new function. WARNING: This does not add the function to the scope (do it manually to add both the mangled variable name and regular function name).
    public Function(string name, VarTypeFunction signature, params ItemAttribute[] attributes) : base(name.Length + name + signature.Mangled(), signature, DataAccessFlags.Read | DataAccessFlags.Static) {
        FuncName = name;
        Attributes = attributes;
        if (!NameMangled) Name = name;
    }

    public void SetScopes(Scope parent) {
        Scope = parent.EnterScope(Name, false);
    }

    public void ResolveVariables() {
        throw new System.NotImplementedException();
    }

    public void ResolveTypes() {
        throw new System.NotImplementedException();
    }

    public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder) {
        throw new System.NotImplementedException();
    }

    public LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        throw new System.NotImplementedException();
    }

}