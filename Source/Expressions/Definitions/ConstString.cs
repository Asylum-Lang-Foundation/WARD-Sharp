using LLVMSharp.Interop;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a constant string.
public class ExpressionConstString : Expression {
    public string Str { get; } // Value of the string.

    // Create a new constant string expression.
    public ExpressionConstString(string str) {
        Type = ExpressionEnum.ConstString;
        Str = str;
    }

    public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
    }

    protected override VarType ReturnType() => VarType.String;

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        return builder.BuildGlobalStringPtr(Str, "W_ConstStr_" + Str);
    }

    public override string ToString() => "\"" + Str + "\"";

}