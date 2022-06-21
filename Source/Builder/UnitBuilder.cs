using WARD.Common;

namespace WARD.Builder;

// For building individual compilation units.
public partial class UnitBuilder {
    internal CompilationUnit Unit = new CompilationUnit(); // Compilation unit that is being built.
    internal string Path; // Path to the unit.

    // Create a new unit builder.
    public UnitBuilder(string path) {
        Path = path;
    }

}