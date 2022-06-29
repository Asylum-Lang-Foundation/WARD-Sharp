using LLVMSharp.Interop;
using WARD.Generics;
using WARD.Scoping;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for resizing an integer.
public class ExpressionResizeInt : Expression {
    public Expression Expression { get; } // Expression to resize.
    public VarTypeInteger NewSize { get; } // New type to resize to.

    // Create a new int resize expression.
    public ExpressionResizeInt(Expression expression, VarTypeInteger newSize) {
        Type = ExpressionEnum.ResizeInt;
        Expression = new ExpressionImplicitCast(expression, Concept.Integer); // Can only operate on integers.
        NewSize = newSize;
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
    }

    protected override VarType ReturnType() => NewSize;

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        VarTypeInteger currType = Expression.GetReturnType() as VarTypeInteger; // We do a cast so this is a fair assumption.
        var exprVal = Expression.CompileRValue(mod, builder, param);
        if (currType.BitWidth > NewSize.BitWidth) {
            return builder.BuildTrunc(exprVal, NewSize.GetLLVMType());
        } else if (currType.BitWidth < NewSize.BitWidth) {
            if (currType.Signed) {
                return builder.BuildSExt(exprVal, NewSize.GetLLVMType());
            } else {
                return builder.BuildZExt(exprVal, NewSize.GetLLVMType());
            }
        } else {
            return exprVal; // LLVM integers have no sign attached, so just return raw value.
        }
    }

    public override string ToString() => "(" + NewSize.ToString() + ")IntResize(" + Expression.ToString() + ")";

}