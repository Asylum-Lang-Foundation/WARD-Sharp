namespace WARD.Expressions;

// Types of expressions.
public enum ExpressionEnum {
    Call,
    ConstInt,
    ConstPointer,
    ConstString,
    LLVM,
    Variable
}