using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Runtime;

namespace WARD.Builder;

// General building.
public partial class ProgramBuilder {

    // Compile the modules.
    public CompiledProgram Compile() {

        // Make sure all the builders have exited.
        EndCompilationUnit(WARDBuilder);
        if (UnitsInProgress.Count > 0) {
            Error.ThrowInternal("Not all of the unit builders have finished building code.");
            return null;
        }

        // Get a list of modules.
        Dictionary<string, LLVMModuleRef> mods = new Dictionary<string, LLVMModuleRef>();
        foreach (var c in CompilationUnits) {
            mods.Add(c.Key, c.Value.Compile(c.Key, RootScope));
        }
        return new CompiledProgram(mods);

    }

}