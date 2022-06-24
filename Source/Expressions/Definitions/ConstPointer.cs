using LLVMSharp.Interop;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a constant pointer.
public class ExpressionConstPointer : Expression {
    public VarTypePointer PointerType { get; } // Type of the pointer to be constant.
    public ulong Value { get; } // Value for the constant pointer to be.

    // Null pointer value.
    public static ExpressionConstPointer NullPointer(VarTypePointer pointerType) => new ExpressionConstPointer(pointerType, 0);

    // Create a new constant pointer expression.
    public ExpressionConstPointer(VarTypePointer pointerType, ulong value) {
        Type = ExpressionEnum.ConstPointer;
        PointerType = pointerType;
        Value = value;
    }

    public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
    }

    protected override VarType ReturnType() => PointerType;

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        return builder.BuildIntToPtr(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, Value, false), PointerType.GetLLVMType());
    }

    public override string ToString() => "0x" + Value.ToString("X");

}