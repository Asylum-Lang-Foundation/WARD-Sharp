using LLVMSharp.Interop;
using WARD.Scoping;

namespace WARD.Statements;

// Context for compiling a section of code.
public class CompilationContext {
    public LLVMPassManagerRef Fpm { get; } // Code optimizer.
    public Scope RootScope { get; } // Root scope.
    public Stack<CodeStatements> CodeStatementsStack = new Stack<CodeStatements>(); // Stack of what code statements.
    public CodeStatements LastBlock; // Last block that was compiled.
    public int InlineCallDepth = 0; // If in an inline call.

    // Create a new context for compilation.
    public CompilationContext(LLVMPassManagerRef fpm, Scope rootScope) {
        Fpm = fpm;
        RootScope = rootScope;
    }

}