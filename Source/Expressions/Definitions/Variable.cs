using LLVMSharp.Interop;
using WARD.Common;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a variable.
public class ExpressionVariable : Expression {
    public string Path { get; } // Path to the variable.
    private List<Variable> PossibleVariables; // Possible variables that may be the resolved one.
    private Variable Resolved = null; // Resolved variable.

    // Create a variable reference.
    public ExpressionVariable(string path) {
        Type = ExpressionEnum.Variable;
        Path = path;
    }

    public override void ResolveVariables() {
        PossibleVariables = Scope.Table.ResolveVariables(Path);
    }

    protected override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = true;
        throw new System.NotImplementedException();
    }

    protected override VarType ReturnType() => Resolved.Type;

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        return Resolved.Value;
    }

    public override string ToString() => Path;

}