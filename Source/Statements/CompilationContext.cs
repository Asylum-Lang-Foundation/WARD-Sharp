namespace WARD.Statements;

// Context for compiling a section of code.
public class CompilationContext {
    public Stack<CodeStatements> CodeStatementsStack = new Stack<CodeStatements>(); // Stack of what code statements.

}