using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Generics;
using WARD.Scoping;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for resizing an integer.
public class ExpressionImplicitCast : Expression {
    public Expression Expression { get; } // Expression to resize.
    public Concept DesiredTypes { get; } // New type to cast to.
    private bool NoCastNeeded = false; // If a cast is needed or not.
    private VarType DestType; // The destination type.

    // Create a new int resize expression.
    public ExpressionImplicitCast(Expression expression, Concept desiredTypes) {
        Type = ExpressionEnum.ImplicitCast;
        Expression = expression;
        DesiredTypes = desiredTypes;
    }

    public override void SetScopes(Scope parent) {
        Expression.SetScopes(parent);
    }

    public override void ResolveVariables() {
        Expression.ResolveVariables();
    }

    public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
        Expression.ResolveTypes();
        var retType = Expression.GetReturnType();
        if (DesiredTypes.TypeFitsConcept(retType)) {
            NoCastNeeded = true; // No action is needed, type already satisfies requirements.
            DestType = retType;
        } else {
            throw new System.NotImplementedException();
        }
    }

    protected override VarType ReturnType() => DestType.GetVarType();

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        if (NoCastNeeded) {
            return Expression.CompileRValue(mod, builder, param);
        } else {
            throw new System.NotImplementedException();
        }
    }

    public override string ToString() => throw new System.NotImplementedException();

}