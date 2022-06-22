using WARD.Common;
using WARD.Exceptions;
using WARD.Scoping;

namespace WARD.Builder;

// File for general builder purposes.
public partial class ProgramBuilder {
    internal Scope RootScope = new Scope(); // Root scope of the program.
    private Dictionary<string, CompilationUnit> CompilationUnits = new Dictionary<string, CompilationUnit>(); // Units to compile in parallel.
    private List<string> UnitsInProgress = new List<string>(); // Units that are being built.

    // Add a compilation unit.
    public void AddCompilationUnit(string path, CompilationUnit compilationUnit) {
        if (CompilationUnits.ContainsKey(path) || UnitsInProgress.Contains(path)) {
            Error.ThrowInternal("Compilation with path \"" + path + "\" has already been added.");
            return;
        }
        CompilationUnits.Add(path, compilationUnit);
    }

    // Export a compilation unit.
    public CompilationUnit ExportCompilationUnit(string path) {
        if (!CompilationUnits.ContainsKey(path)) {
            Error.ThrowInternal("Compilation with path \"" + path + "\" does not exist or has not finished building.");
            return null;
        }
        return CompilationUnits[path];
    }

    // Start a new compilation unit.
    public UnitBuilder NewCompilationUnit(string path) {
        if (CompilationUnits.ContainsKey(path) || UnitsInProgress.Contains(path)) {
            Error.ThrowInternal("Compilation with path \"" + path + "\" has already been added.");
            return null;
        }
        UnitsInProgress.Add(path);
        return new UnitBuilder(path);
    }

    // End a compilation unit.
    public void EndCompilationUnit(UnitBuilder unitBuilder) {
        UnitsInProgress.Remove(unitBuilder.Path);
        CompilationUnits.Add(unitBuilder.Path, unitBuilder.Unit);
        RootScope.ImportScope(unitBuilder.RootScope);
        unitBuilder.Dispose();
    }

}