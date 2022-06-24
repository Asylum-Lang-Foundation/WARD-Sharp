using LLVMSharp.Interop;
using WARD.Exceptions;

namespace WARD.Runtime;

// Export LLVM and object bitcode.
public partial class CompiledProgram {

    // Export LLVM assembly.
    public void ExportLLVMAssembly(string unit, string path) {
        if (!Mods.ContainsKey(unit)) {
            Error.ThrowInternal("Compiled program does not contain compilation unit \"" + unit + "\"");
            return;
        }
        Mods[unit].PrintToFile(path);
    }

    // Export LLVM bitcode.
    public void ExportLLVMBitcode(string unit, string path) {
        if (!Mods.ContainsKey(unit)) {
            Error.ThrowInternal("Compiled program does not contain compilation unit \"" + unit + "\"");
            return;
        }
        Mods[unit].WriteBitcodeToFile(path);
    }

    // Export assembly file.
    public void ExportAssembly(string unit, string path, string triple = null, string cpu = "generic", string features = "", LLVMCodeGenOptLevel opt = LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault, LLVMRelocMode reloc = LLVMRelocMode.LLVMRelocDefault, LLVMCodeModel model = LLVMCodeModel.LLVMCodeModelDefault) {
        if (!Mods.ContainsKey(unit)) {
            Error.ThrowInternal("Compiled program does not contain compilation unit \"" + unit + "\"");
            return;
        }
        ExecutionEngine.InitializeAllTargets();
        if (triple == null) triple = LLVMTargetRef.DefaultTriple;
        var target = LLVMTargetRef.GetTargetFromTriple(triple);
        var machine = target.CreateTargetMachine(triple, cpu, features, opt, reloc, model);
        LLVMModuleRef mod = Mods[unit];
        mod.Target = triple;
        machine.EmitToFile(mod, path, LLVMCodeGenFileType.LLVMAssemblyFile);
    }

    // Export object file.
    public void ExportObject(string unit, string path, string triple = null, string cpu = "generic", string features = "", LLVMCodeGenOptLevel opt = LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault, LLVMRelocMode reloc = LLVMRelocMode.LLVMRelocDefault, LLVMCodeModel model = LLVMCodeModel.LLVMCodeModelDefault) {
        if (!Mods.ContainsKey(unit)) {
            Error.ThrowInternal("Compiled program does not contain compilation unit \"" + unit + "\"");
            return;
        }
        ExecutionEngine.InitializeAllTargets();
        if (triple == null) triple = LLVMTargetRef.DefaultTriple;
        var target = LLVMTargetRef.GetTargetFromTriple(triple);
        var machine = target.CreateTargetMachine(triple, cpu, features, opt, reloc, model);
        LLVMModuleRef mod = Mods[unit];
        mod.Target = triple;
        machine.EmitToFile(mod, path, LLVMCodeGenFileType.LLVMObjectFile);
    }

}