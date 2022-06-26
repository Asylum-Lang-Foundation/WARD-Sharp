using LLVMSharp.Interop;
using WARD.Expressions;
using WARD.Scoping;
using WARD.Statements;
using WARD.Types;

namespace WARD.Operators;

// For times when you need to summon an operator.
public class ExpressionOperator : Expression {
    public string Op { get; } // Operator to use.
    public Expression[] Args { get; } // Arguments for the operator.
    private FunctionGeneric Resolved; // Resolved operator to call.

    // Create a new operator expression.
    public ExpressionOperator(string op, params Expression[] args) {
        Type = ExpressionEnum.Operator;
        Op = op;
        Args = args;
    }

    public override void SetScopes(Scope parent) {
        throw new System.NotImplementedException();
    }

    public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
        throw new System.NotImplementedException();
    }

    protected override VarType ReturnType() => throw new System.NotImplementedException();

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        throw new System.NotImplementedException();
    }

    public override string ToString() => throw new System.NotImplementedException();

}