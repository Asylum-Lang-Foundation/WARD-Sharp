using LLVMSharp.Interop;

namespace WARD.Runtime;

// For handling built LLVM modules.
public partial class CompiledProgram {
    private Dictionary<string, LLVMModuleRef> Mods; // Compiled modules.

    // Create a new compiled program.
    public CompiledProgram(Dictionary<string, LLVMModuleRef> mods) {
        Mods = mods;
    }

}