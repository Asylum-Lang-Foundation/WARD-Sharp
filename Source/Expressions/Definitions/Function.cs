using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for a function. Useful if you want to get a particular mangling of a function.
public class ExpressionFunction : Expression {
    public string Path { get; } // Path to the function.
    public VarTypeFunction FunctionType { get; } // Type of the function to fetch.
    private Function Resolved; // Resolved function.

    // Create a function reference.
    public ExpressionFunction(string path, VarTypeFunction functionType) {
        Type = ExpressionEnum.Function;
        Path = path;
        FunctionType = functionType;
    }

    public override void ResolveVariables() {
        string mangledName = "TODO: MANGLED NAME HERE!!!";
        Resolved = Scope.Table.ResolveVariable(mangledName) as Function;
        if (Resolved == null) {
            Error.ThrowInternal("Can not resolve function with mangled name \"" + mangledName + "\".");
        }
    }

    public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false; // Functions don't get loaded.
        if (!FunctionType.Equals(Resolved.Type.GetVarType())) {
            Error.ThrowInternal("Resolved function \"" + Path + "\" does not match requested type.");
        }
    }

    protected override VarType ReturnType() => Resolved.Type.GetVarType();

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        return Resolved.Compile(mod, builder, param);
    }

    public override string ToString() => Path;

}