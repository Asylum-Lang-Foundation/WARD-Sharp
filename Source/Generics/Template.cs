using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Scoping;
using WARD.Statements;

namespace WARD.Generics;

// Template scope for having generic code. TODO: VARIADIC PARAMETERS!!!
public class Template : ICompileable {
    public TemplateItem[] Items { get; } // Template items.
    public FileContext FileContext { get; } // Context for the template.

    public FileContext GetFileContext() => FileContext;

    // Create a new template.
    public Template(params TemplateItem[] items) {
        Items = items;
    }

    public void SetScopes(Scope parent) {
        throw new NotImplementedException();
    }

    public void ResolveVariables() {
        throw new NotImplementedException();
    }

    public void ResolveTypes() {
        throw new NotImplementedException();
    }

    public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder) {
        throw new NotImplementedException();
    }

    public LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj) {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode() {
        throw new System.NotImplementedException();
    }

}