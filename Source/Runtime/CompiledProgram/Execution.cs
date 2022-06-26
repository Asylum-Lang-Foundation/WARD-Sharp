using LLVMSharp.Interop;
using WARD.Statements;

namespace WARD.Runtime;

// Execute the program or functions from it.
public partial class CompiledProgram {

    // Execute the main function of the compiled program.
    public int Execute(string[] envp = null, params string[] args) {

        // Initialize engine.
        ExecutionEngine.InitializeAllTargets();
        var exe = Mods.ElementAt(0).Value.CreateMCJITCompiler();

        // Set proper envp if none and execute main.
        if (envp == null) envp = new string[0];
        LLVMValueRef main = Mods.ElementAt(0).Value.GetNamedFunction("main");
        return exe.RunFunctionAsMain(main, (uint)args.Length, args, envp);
    }

    // Get a function to execute from the compiled program. NOTE: Does not support variadic arguments!
    public TDelegate GetFunctionExecuter<TDelegate>(Function function) {

        // Initialize engine.
        ExecutionEngine.InitializeAllTargets();
        var exe = Mods.ElementAt(0).Value.CreateMCJITCompiler();

        // Get args and function.
        LLVMValueRef func = Mods.ElementAt(0).Value.GetNamedFunction(function.ToString());
        var exeFunc = exe.GetPointerToGlobal<TDelegate>(func);
        return exeFunc;

    }

}