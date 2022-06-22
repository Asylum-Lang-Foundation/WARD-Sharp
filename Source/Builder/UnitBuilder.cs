using WARD.Common;
using WARD.Exceptions;
using WARD.Scoping;

namespace WARD.Builder;

// For building individual compilation units.
public partial class UnitBuilder : IDisposable {
    private bool Disposed = false; // If the builder has been disposed or not.
    internal CompilationUnit Unit = new CompilationUnit(); // Compilation unit that is being built.
    internal Scope RootScope = new Scope(); // Root scope to eventually be joined to the program's root scope.
    internal Scope CurrentScope; // Current scope that is being accessed.
    internal string Path; // Path to the unit.

    // Create a new unit builder.
    public UnitBuilder(string path) {
        Path = path;
        CurrentScope = RootScope;
    }

    // Dispose of the builder.
    public void Dispose() {
        if (Disposed) {
            Error.ThrowInternal("Unit builder has already been disposed of.");
        }
        Disposed = true;
    }

}