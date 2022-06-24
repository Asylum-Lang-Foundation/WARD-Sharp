using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Runtime;

// Execute the program or functions from it.
public partial class CompiledProgram {

    // Execute the main function of the compiled program.
    public int Execute(string[] envp = null, params string[] args) {
        ExecutionEngine.InitializeAllTargets();
        var exe = Mods.ElementAt(0).Value.CreateMCJITCompiler();
        if (envp == null) envp = new string[0];
        LLVMValueRef main = Mods.ElementAt(0).Value.GetNamedFunction("main");
        return exe.RunFunctionAsMain(main, (uint)args.Length, args, envp);
    }

    /*

    // Execute a function from the compiled program.
    public T ExecuteFunction<T>(string name, FunctionType signature, params object[] args) {
        ExecutionEngine.InitializeAllTargets();
        var exe = Mods.ElementAt(0).Value.CreateMCJITCompiler();
        if (args == null) args = new object[0];
        LLVMValueRef func = Mods.ElementAt(0).Value.GetNamedFunction(name);
        delegate realFuncSig;
        var exeFunc = exe.GetPointerToGlobal<realFuncSig>(func);
        exeFunc();
    }

    */

}