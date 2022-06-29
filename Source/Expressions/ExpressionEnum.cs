namespace WARD.Expressions;

// Types of expressions.
public enum ExpressionEnum {
    Call,
    ConstInt,
    ConstPointer,
    ConstString,
    Function,
    ImplicitCast,
    LLVM,
    Operator,
    ResizeInt,
    Variable,
    NumExpressionTypes
}