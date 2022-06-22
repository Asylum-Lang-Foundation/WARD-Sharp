using WARD.Exceptions;
using WARD.Scoping;
using WARD.Statements;

namespace WARD.Builder;

// For building code statements.
public partial class CodeBuilder : IDisposable {
    private bool Disposed = false; // If the builder has been disposed or not.
    internal CodeStatements Statements = new CodeStatements(); // Statements to append to.
    internal Scope RootScope = new Scope(); // Scope of the code statements. Since resolving is done later, it doesn't matter that we are referring to things that don't exist atm.
    internal Scope CurrentScope; // Current scope to modify.

    // Create a new code builder.
    public CodeBuilder() {
        CurrentScope = RootScope.EnterScope("%CODESTATEMENTS%_" + Statements.Instance);
    }

    // Finish building and return the code statements.
    public Tuple<CodeStatements, Scope> EndBuilding() {
        Dispose();
        return new Tuple<CodeStatements, Scope>(Statements, RootScope);
    }

    // Dispose of the builder.
    public void Dispose() {
        if (Disposed) {
            Error.ThrowInternal("Code builder has already been disposed of.");
        }
        Disposed = true;
    }

}