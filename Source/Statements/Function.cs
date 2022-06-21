using LLVMSharp.Interop;
using WARD.Common;
using WARD.Exceptions;
using WARD.Types;

namespace WARD.Statements;

// Create a function. TODO: FUNCTION FOR DECLARING DEFINTION!!!
public class Function : Variable, ICompileableTopLevel {
    public ItemAttribute[] Attributes { get; } // Function attributes for additional rules for handling.
    public bool Inline { get; } = false; // If the function serves as a macro rather than being defined.
    public CodeStatements Definition { get; internal set; } = null; // Function definition if one exists.
    public FileContext FileContext { get; } // File context of where the function is.
    public FileContext GetFileContext() => FileContext;

    // Create a new function. WARNING: This does not add the function to the scope (do it manually to add both the mangled variable name and regular function name).
    public Function(string name, VarTypeFunction signature, params ItemAttribute[] attributes) : base(signature.Mangled(), signature, DataAccessFlags.Read | DataAccessFlags.Static) {
        Attributes = attributes;
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

    // Compile the item.
    public LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        throw new System.NotImplementedException();
    }

}