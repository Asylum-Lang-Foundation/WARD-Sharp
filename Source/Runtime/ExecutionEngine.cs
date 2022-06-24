using LLVMSharp.Interop;

namespace WARD.Runtime;

// For holding LLVM execution info. // TODO: REST OF TARGETS!
public static class ExecutionEngine {
    private static bool TargetsInitialized = false; // If targets have been initialized.

    // Initialize ARM target.
    private static void InitializeARMTarget() {
        // TODO!!!
    }

    // Initialize x86 target.
    private static void InitializeX86Target() {
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86TargetMC();
        LLVM.InitializeX86AsmParser();
        LLVM.InitializeX86AsmPrinter();
    }

    // Initialize all of the targets.
    public static void InitializeAllTargets() {
        if (TargetsInitialized) return;
        InitializeX86Target();
        LLVM.LinkInMCJIT();
        LLVM.InitializeNativeTarget();
        LLVM.InitializeNativeAsmPrinter();
        LLVM.InitializeNativeAsmParser();
        TargetsInitialized = true;
    }

}