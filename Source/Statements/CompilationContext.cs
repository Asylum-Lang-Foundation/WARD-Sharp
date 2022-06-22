using LLVMSharp.Interop;

namespace WARD.Statements;

// Context for compiling a section of code.
public class CompilationContext {
    public LLVMPassManagerRef Fpm { get; } // Code optimizer.
    public Stack<CodeStatements> CodeStatementsStack = new Stack<CodeStatements>(); // Stack of what code statements.
    public CodeStatements LastBlock; // Last block that was compiled.

    // Create a new context for compilation.
    public CompilationContext(LLVMPassManagerRef fpm) {
        Fpm = fpm;
    }

}