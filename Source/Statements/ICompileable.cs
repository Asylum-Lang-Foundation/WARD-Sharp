using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Scoping;

namespace WARD.Statements;

// Any item that can be compiled.
public interface ICompileable {

    // Get the context where the item is from.
    FileContext GetFileContext();

    // Set the scopes for this item and children given the parent scope.
    void SetScopes(Scope parent);

    // Resolve variable definitions.
    void ResolveVariables();

    // Resolve types and if there are overloads of variables merge to one.
    void ResolveTypes();

    // Compile variable declarations as stack memory can only be allocated in the beginning of a function.
    void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder);

    // Compile the item.
    LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param);

}

// Define varient for top level statements.
public interface ICompileableTopLevel : ICompileable {}