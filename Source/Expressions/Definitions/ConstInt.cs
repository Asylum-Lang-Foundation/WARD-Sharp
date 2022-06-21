using LLVMSharp.Interop;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a constant integer.
public class ExpressionConstInt : Expression {
    public bool Signed;
    public uint BitWidth;
    public ulong Value;

    // Create a new constant int expression.
    public ExpressionConstInt(bool signed, uint bitWidth, ulong value) {
        Type = ExpressionEnum.ConstInt;
        Signed = signed;
        BitWidth = bitWidth;
        Value = value;
    }

    protected override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
    }

    protected override VarType ReturnType() => new VarTypeInteger(Signed, BitWidth);

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
        return LLVMValueRef.CreateConstInt(LLVMTypeRef.CreateInt(BitWidth), Value, Signed);
    }

}