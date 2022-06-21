using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Expressions;
using WARD.Types;

namespace WARD.Statements;

// Return a value.
public class StatementReturn : ICompileable {
    public Expression ReturnValue { get; }
    public FileContext FileContext;

    public FileContext GetFileContext() => FileContext;

    // Create a new return statement.
    public StatementReturn(Expression returnValue) {
        ReturnValue = returnValue;
    }

    public void ResolveVariables() {
        if (ReturnValue != null) ReturnValue.ResolveVariables();
    }

    public void ResolveTypes() {
        if (ReturnValue != null) ReturnValue.ResolveTypes();
    }

    public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder) {
        if (ReturnValue != null) ReturnValue.CompileDeclarations(mod, builder);
    }

    public LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {

        // Sanity check.
        if (param.CodeStatementsStack.Count < 1) {
            Error.ThrowInternal("Can not return a value while not in a code statements list.");
            return null;
        }

        // Return a value.
        if (ReturnValue == null) {
            LLVMValueRef ret = builder.BuildRetVoid();
            param.CodeStatementsStack.Peek().ReturnAValue(ret);
            return ret;
        } else if (ReturnValue.Equals(VarType.Void)) {
            LLVMValueRef ret = builder.BuildRetVoid();
            param.CodeStatementsStack.Peek().ReturnAValue(ret);
            return ret;
        } else {
            LLVMValueRef ret = ReturnValue.CompileRValue(mod, builder, param);
            builder.BuildRet(ret);
            param.CodeStatementsStack.Peek().ReturnAValue(ret);
            return ret;
        }

    }

    public override string ToString() {
        string ret = "return";
        if (ReturnValue != null) ret += " " + ReturnValue.ToString();
        return ret + ";";
    }

}