using LLVMSharp.Interop;
using WARD.Exceptions;

namespace WARD.Common;

// Any item that can be compiled.
public interface ICompileable {

    // Get the context where the item is from.
    FileContext GetFileContext();

    // Resolve variable definitions.
    void ResolveVariables();

    // Resolve types and if there are overloads of variables merge to one.
    void ResolveTypes();

    // Compile variable declarations as stack memory can only be allocated in the beginning of a function.
    void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder, object param);

    // Compile the item.
    LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param);

}