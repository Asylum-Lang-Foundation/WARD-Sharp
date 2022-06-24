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
    public void AddCompilationUnit(string unit, CompilationUnit compilationUnit) {
        if (CompilationUnits.ContainsKey(unit) || UnitsInProgress.Contains(unit)) {
            Error.ThrowInternal("Compilation with unit \"" + unit + "\" has already been added.");
            return;
        }
        CompilationUnits.Add(unit, compilationUnit);
    }

    // Export a compilation unit.
    public CompilationUnit ExportCompilationUnit(string unit) {
        if (!CompilationUnits.ContainsKey(unit)) {
            Error.ThrowInternal("Compilation with unit \"" + unit + "\" does not exist or has not finished building.");
            return null;
        }
        return CompilationUnits[unit];
    }

    // Start a new compilation unit.
    public UnitBuilder NewCompilationUnit(string unit) {
        if (CompilationUnits.ContainsKey(unit) || UnitsInProgress.Contains(unit)) {
            Error.ThrowInternal("Compilation with unit \"" + unit + "\" has already been added.");
            return null;
        }
        UnitsInProgress.Add(unit);
        return new UnitBuilder(unit);
    }

    // End a compilation unit.
    public void EndCompilationUnit(UnitBuilder unitBuilder) {
        UnitsInProgress.Remove(unitBuilder.Path);
        CompilationUnits.Add(unitBuilder.Path, unitBuilder.Unit);
        RootScope.ImportScope(unitBuilder.RootScope);
        unitBuilder.Dispose();
    }

}