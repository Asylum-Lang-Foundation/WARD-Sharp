using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Low level operation that can be compiled directly to LLVM bitcode.
public abstract class Expression : ICompileable {
    public ExpressionEnum Type { get; protected set; } // The type of expression.
    public bool LValue { get; protected set; } = true; // If the value should be loaded or not when compiling.
    public FileContext FileContext = null; // File context for the expression.

    public FileContext GetFileContext() => FileContext;

    // Resolve types.
    public void ResolveTypes() => ResolveTypes(null, null);

    // Get the return type. Use this instead of ReturnType.
    public VarType GetReturnType() => ReturnType().GetVarType();

    // Compile to an L-value is null if not a valid L-value.
    public LLVMValueRef CompileLValue(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        if (!LValue) return null;
        else return Compile(mod, builder, param);
    }

    // Compile an expression into an R-value by loading from an L-value if needed.
    public LLVMValueRef CompileRValue(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        LLVMValueRef ret = Compile(mod, builder, param);
        if (LValue) return builder.BuildLoad(ret, "LToRValue");
        else return ret;
    }

    // Vfunctions.
    public virtual void ResolveVariables() {} // Resolve variable and function call references to a list of possibilities.
    protected virtual void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {} // Resolve types, type check, add casts, and solidify all function references. The parameters are so calls can have expressions resolve the correct function.
    protected abstract VarType ReturnType(); // Get the return type of an expression.
    public abstract LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param); // Compile the expression.
    public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder) {} // Compile any variable definitions.

}