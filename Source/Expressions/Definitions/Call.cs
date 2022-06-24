using LLVMSharp.Interop;
using WARD.Common;
using WARD.Scoping;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a function call.
public class ExpressionCall : Expression {
    public Expression Callee { get; } // What to call.
    public Expression[] Args { get; } // Arguments to the function call.
    private VarTypeFunction FuncType { get; } // Type of the function being called.

    // Create a new function call.
    public ExpressionCall(Expression callee, params Expression[] args) {
        Type = ExpressionEnum.Call;
        Callee = callee;
        Args = args;
    }

    public override void SetScopes(Scope parent) {
        Callee.SetScopes(parent);
        foreach (var a in Args) {
            a.SetScopes(parent);
        }
    }

    public override void ResolveVariables() {
        Callee.ResolveVariables();
        foreach (var a in Args) {
            a.ResolveVariables();
        }
    }

    protected override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
        throw new System.NotImplementedException();
    }

    protected override VarType ReturnType() => FuncType.ReturnType.GetVarType();

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        var callee = Callee.CompileLValue(mod, builder, param);
        LLVMValueRef[] args = new LLVMValueRef[Args.Length];
        for (int i = 0; i < args.Length; i++) {
            args[i] = Args[i].CompileRValue(mod, builder, param);
        }
        return builder.BuildCall(callee, args);
    }

    public override string ToString() {
        string ret = "(" + ReturnType().ToString() + ")" + Callee.ToString() + "(";
        for (int i = 0; i < Args.Length; i++) {
            ret += Args[i].ToString();
            if (i != Args.Length - 1) {
                ret += ", ";
            }
        }
        return ret + ")";
    }

}