using LLVMSharp.Interop;
using WARD.Scoping;
using WARD.Statements;

namespace WARD.Common;

// A collection of definitions to be compiled.
public class CompilationUnit {
    public List<ICompileableTopLevel> Items = new List<ICompileableTopLevel>(); // Items to compile.

    // Compile the unit.
    internal LLVMModuleRef Compile(string modName, Scope rootScope) {

        // Prepare for compilation.
        foreach (var item in Items) {
            item.SetScopes(rootScope);
        }
        foreach (var item in Items) {
            item.ResolveVariables();
        }
        foreach (var item in Items) {
            item.ResolveTypes();
        }

        // Initialize module and optimizer.
        var mod = LLVMModuleRef.CreateWithName(modName);
        var fpm = mod.CreateFunctionPassManager();
        fpm.AddInstructionCombiningPass();
        fpm.AddReassociatePass();
        fpm.AddNewGVNPass();
        fpm.AddCFGSimplificationPass();
        fpm.InitializeFunctionPassManager();

        // Compile the module.
        var builder = LLVMBuilderRef.Create(mod.Context);
        foreach (var item in Items) {
            item.Compile(mod, builder, new CompilationContext(fpm));
        }
        return mod;

    }

}