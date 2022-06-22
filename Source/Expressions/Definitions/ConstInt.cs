using LLVMSharp.Interop;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a constant integer.
public class ExpressionConstInt : Expression {
    public VarTypeInteger IntType { get; } // Type of integer that is constant.
    public ulong Value { get; } // Value of the constant integer.

    // Create a new constant int expression.
    public ExpressionConstInt(VarTypeInteger intType, ulong value) {
        Type = ExpressionEnum.ConstInt;
        IntType = intType;
        Value = value;
    }

    protected override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
    }

    protected override VarType ReturnType() => IntType;

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        return LLVMValueRef.CreateConstInt(IntType.GetLLVMType(), Value, IntType.Signed);
    }

    public override string ToString() => "((" + IntType.ToString() + ")" + (IntType.Signed ? ((long)Value).ToString() : Value.ToString()) + ")";

}