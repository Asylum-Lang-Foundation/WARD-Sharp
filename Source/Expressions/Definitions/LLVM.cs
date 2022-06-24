using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Scoping;
using WARD.Statements;
using WARD.Types;

namespace WARD.Expressions;

// Expression for LLVM.
public class ExpressionLLVM : Expression {
    public string Instruction { get; } // LLVM assembly instruction to execute.
    public VarType RetType { get; } // Type that is going to be returned.
    public Expression[] Args { get; } // Expressions that are arguments to the LLVM function.

    // Create a new LLVM expression. WARNING: Types are not checked, only use this if you know what you are doing!
    public ExpressionLLVM(string instruction, VarType retType, params Expression[] args) {
        Type = ExpressionEnum.LLVM;
        RetType = retType;
        Args = args;
    }

    public override void SetScopes(Scope parent) {
        foreach (var a in Args) {
            a.SetScopes(parent);
        }
    }

    public override void ResolveVariables() {
        foreach (var a in Args) {
            a.ResolveVariables();
        }
    }

    protected override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
        foreach (var a in Args) {
            a.ResolveTypes();
        }
    }

    protected override VarType ReturnType() => RetType.GetVarType();

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {

        // Compile arguments first.
        LLVMValueRef[] args = new LLVMValueRef[Args.Length];
        for (int i = 0; i < args.Length; i++) {
            args[i] = Args[i].CompileRValue(mod, builder, param);
        }

        // Execute instruction.
        switch (Instruction) {
            case "add":
                VerifyArgs(2);
                return builder.BuildAdd(args[0], args[1]);
            case "addnsw":
                VerifyArgs(2);
                return builder.BuildNSWAdd(args[0], args[1]);
            case "addnuw":
                VerifyArgs(2);
                return builder.BuildNUWAdd(args[0], args[1]);
            case "and":
                VerifyArgs(2);
                return builder.BuildAnd(args[0], args[1]);
            case "ashr":
                VerifyArgs(2);
                return builder.BuildAShr(args[0], args[1]);
            case "call":
                return builder.BuildCall(args[0], args.ToList().GetRange(1, args.Length - 1).ToArray());
            case "extractelement":
                VerifyArgs(2);
                return builder.BuildExtractElement(args[0], args[1]);
            case "fadd":
                VerifyArgs(2);
                return builder.BuildFAdd(args[0], args[1]);
            case "fcmpfalse":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealPredicateFalse, args[0], args[1]);
            case "fcmpoeq":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealOEQ, args[0], args[1]);
            case "fcmpoge":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealOGE, args[0], args[1]);
            case "fcmpogt":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealOGT, args[0], args[1]);
            case "fcmpole":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealOLE, args[0], args[1]);
            case "fcmpolt":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealOLT, args[0], args[1]);
            case "fcmpone":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealONE, args[0], args[1]);
            case "fcmpord":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealORD, args[0], args[1]);
            case "fcmptrue":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealPredicateTrue, args[0], args[1]);
            case "fcmpueq":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealUEQ, args[0], args[1]);
            case "fcmpuge":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealUGE, args[0], args[1]);
            case "fcmpugt":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealUGT, args[0], args[1]);
            case "fcmpule":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealULE, args[0], args[1]);
            case "fcmpult":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealULT, args[0], args[1]);
            case "fcmpune":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealUNE, args[0], args[1]);
            case "fcmpuno":
                VerifyArgs(2);
                return builder.BuildFCmp(LLVMRealPredicate.LLVMRealUNO, args[0], args[1]);
            case "fdiv":
                VerifyArgs(2);
                return builder.BuildFDiv(args[0], args[1]);
            case "fmul":
                VerifyArgs(2);
                return builder.BuildFMul(args[0], args[1]);
            case "fneg":
                VerifyArgs(1);
                return builder.BuildFNeg(args[0]);
            case "freeze":
                VerifyArgs(1);
                return builder.BuildFreeze(args[0]);
            case "frem":
                VerifyArgs(2);
                return builder.BuildFRem(args[0], args[1]);
            case "fsub":
                VerifyArgs(2);
                return builder.BuildFSub(args[0], args[1]);
            case "gep":
                return builder.BuildGEP(args[0], args.ToList().GetRange(1, args.Length - 1).ToArray());
            case "icmpeq":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntEQ, args[0], args[1]);
            case "icmpne":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntNE, args[0], args[1]);
            case "icmpsge":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntSGE, args[0], args[1]);
            case "icmpsgt":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntSGT, args[0], args[1]);
            case "icmpsle":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntSLE, args[0], args[1]);
            case "icmpslt":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntSLT, args[0], args[1]);
            case "icmpuge":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntUGE, args[0], args[1]);
            case "icmpugt":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntUGT, args[0], args[1]);
            case "icmpule":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntULE, args[0], args[1]);
            case "icmpult":
                VerifyArgs(2);
                return builder.BuildICmp(LLVMIntPredicate.LLVMIntULT, args[0], args[1]);
            case "insertelement":
                VerifyArgs(3);
                return builder.BuildInsertElement(args[0], args[1], args[2]);
            case "load":
                VerifyArgs(1);
                return builder.BuildLoad(args[0]);
            case "lshr":
                VerifyArgs(2);
                return builder.BuildLShr(args[0], args[1]);
            case "mul":
                VerifyArgs(2);
                return builder.BuildMul(args[0], args[1]);
            case "mulnsw":
                VerifyArgs(2);
                return builder.BuildNSWMul(args[0], args[1]);
            case "mulnuw":
                VerifyArgs(2);
                return builder.BuildNUWMul(args[0], args[1]);
            case "or":
                VerifyArgs(2);
                return builder.BuildOr(args[0], args[1]);
            case "sdiv":
                VerifyArgs(2);
                return builder.BuildSDiv(args[0], args[1]);
            case "sdivexact":
                VerifyArgs(2);
                return builder.BuildExactSDiv(args[0], args[1]);
            case "select":
                VerifyArgs(3);
                return builder.BuildSelect(args[0], args[1], args[2]);
            case "shl":
                VerifyArgs(2);
                return builder.BuildShl(args[0], args[1]);
            case "shufflevector":
                VerifyArgs(3);
                return builder.BuildShuffleVector(args[0], args[1], args[2]);
            case "srem":
                VerifyArgs(2);
                return builder.BuildSRem(args[0], args[1]);
            case "store":
                VerifyArgs(2);
                return builder.BuildStore(args[0], args[1]);
            case "sub":
                VerifyArgs(2);
                return builder.BuildSub(args[0], args[1]);
            case "subnsw":
                VerifyArgs(2);
                return builder.BuildNSWSub(args[0], args[1]);
            case "subnuw":
                VerifyArgs(2);
                return builder.BuildNUWSub(args[0], args[1]);
            case "udiv":
                VerifyArgs(2);
                return builder.BuildUDiv(args[0], args[1]);
            case "urem":
                VerifyArgs(2);
                return builder.BuildURem(args[0], args[1]);
            case "xor":
                VerifyArgs(2);
                return builder.BuildXor(args[0], args[1]);
            default:
                Error.ThrowInternal("Unknown LLVM instruction \"" + Instruction + "\".");
                return null;
        }

        // Verify the arguments count.
        void VerifyArgs(int num) {
            if (num != Args.Length) {
                Error.ThrowInternal("LLVM assembly call with an invalid number of arguments. Instruction \"" + Instruction + "\" expects " + num + " arguments, but " + Args.Length + " where given.");
            }
        }

    }

    public override string ToString() {
        string ret = "(" + RetType.ToString() + ")LLVM(";
        for (int i = 0; i < Args.Length; i++) {
            ret += Args[i].ToString();
            if (i != Args.Length - 1) ret += ", ";
        }
        return ret + ")";
    }

}