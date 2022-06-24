using LLVMSharp.Interop;
using WARD.Common;
using WARD.Exceptions;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a variable.
// NOTE: Function pointers will need more work to be resolved when & is implemented.
// Consider: func<int, float, int>* ptr = &doThing;
// The resolve type parameters must be handed through the & operator to the doThing reference for this to work.
public class ExpressionVariable : Expression {
    public string Path { get; } // Path to the variable.
    private Variable PossibleVariable; // Possible variable that may be the resolved one.
    private List<Function> PossibleFunctions; // Possible functions that may be the resolved one.
    private Variable Resolved; // Resolved variable.

    // Create a variable reference.
    public ExpressionVariable(string path) {
        Type = ExpressionEnum.Variable;
        Path = path;
    }

    public override void ResolveVariables() {
        PossibleVariable = Scope.Table.ResolveVariable(Path);
        PossibleFunctions = Scope.Table.ResolveFunctions(Path);
    }

    public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {

        // Parameters are not null, this means we are resolving a function.
        // What if we are resolving a function pointer?
        // This won't happen, as we must *dereference* the function pointer first.
        // The dereference operator is calling this function, thus it won't give us populated type info for resolution.
        if (parameterTypes != null) {
            LValue = false; // Functions don't get loaded as they are really R-values.

            // An exact match has been found, function is probably unmangled.
            if (PossibleVariable != null) {
                Resolved = PossibleVariable;
                return;
            }

            // Time for some function overload resolution.
            List<Function> applicableFunctions = new List<Function>();
            throw new System.NotImplementedException();

        }

        // Otherwise, we are simply just fetching a variable.
        else {
            LValue = true; // Variables are L-values.
            if (PossibleVariable != null) {
                Resolved = PossibleVariable;
            } else {
                Error.ThrowInternal("Can not resolve variable with path \"" + Path + "\".");
            }
        }

    }

    protected override VarType ReturnType() => Resolved.Type.GetVarType();

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        var func = Resolved as Function;
        if (func != null) {
            return func.Compile(mod, builder, param);
        }
        return Resolved.Value;
    }

    public override string ToString() => Path;

}