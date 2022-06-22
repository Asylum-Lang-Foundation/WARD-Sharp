using LLVMSharp.Interop;

namespace WARD.Execution;

// For holding LLVM execution info.
public static class ExecutionEngine {

    // Static constructor.
    static ExecutionEngine() {
        LLVM.LinkInMCJIT();
        LLVM.InitializeNativeTarget();
        LLVM.InitializeNativeAsmPrinter();
        LLVM.InitializeNativeAsmParser();
    }

}