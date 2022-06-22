using LLVMSharp.Interop;

namespace WARD.Runtime;

// Execute the program or functions from it.
public partial class CompiledProgram {

    // Execute the main function of the compiled program.
    public int Execute(string[] args = null, string[] envp = null) {
        var exe = Mods.ElementAt(0).Value.CreateExecutionEngine();
        if (args == null) args = new string[0];
        if (envp == null) envp = new string[0];
        LLVMValueRef main = Mods.ElementAt(0).Value.GetNamedFunction("main");
        return exe.RunFunctionAsMain(main, (uint)args.Length, args, envp);
    }

}