using System.Collections.Generic;
using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Expressions;
using WARD.Scoping;
using WARD.Statements;
using WARD.Types;

namespace WARD.Operators;

// For times when you need to summon an operator.
public class ExpressionOperator : Expression {
    public string Op { get; } // Operator to use.
    public Expression[] Args { get; } // Arguments for the operator.
    private List<FunctionGeneric> PossibleOperators; // Possible operators.
    private Dictionary<string, VarType> TemplateParameters; // Template parameters for the resolved function.
    private FunctionGeneric BestFit; // Best fit generic function.
    private Function Resolved; // Resolved operator to call.

    // Create a new operator expression.
    public ExpressionOperator(string op, params Expression[] args) {
        Type = ExpressionEnum.Operator;
        Op = op;
        Args = args;
    }

    public override void SetScopes(Scope parent) {
        Scope = parent;
        foreach (var a in Args) {
            a.SetScopes(parent);
        }
    }

    public override void ResolveVariables() {
        foreach (var a in Args) {
            a.ResolveVariables();
        }
        PossibleOperators = Scope.Table.ResolveOperator(Op);
    }

    public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
        LValue = false;
        VarType[] argTypes = new VarType[Args.Length];
        for (int i = 0; i < argTypes.Length; i++) {
            Args[i].ResolveTypes();
            argTypes[i] = Args[i].GetReturnType();
        }
        List<Tuple<int, Dictionary<string, VarType>, FunctionGeneric>> options = new List<Tuple<int, Dictionary<string, VarType>, FunctionGeneric>>();
        foreach (var o in PossibleOperators) {
            int distance;
            Dictionary<string, VarType> typeDefines = new Dictionary<string, VarType>();
            if (o.CallSatisfiesTemplate(argTypes, out distance, out typeDefines)) {
                options.Add(new Tuple<int, Dictionary<string, VarType>, FunctionGeneric>(distance, typeDefines, o));
            }
        }
        if (options.Count == 0) {
            Error.ThrowInternal("No valid overload found for \"" + Op + "\" operator.");
        }
        var sorted = options.OrderBy(x => x.Item1);
        var bestFit = sorted.First();
        if (sorted.Where(x => x.Item1 == bestFit.Item1).Count() > 1) {
            Error.ThrowInternal("Multiple generic overloads possible.");
        }
        Resolved = bestFit.Item3.Instantiate(bestFit.Item2);
        TemplateParameters = bestFit.Item2;
        BestFit = bestFit.Item3;
    }

    protected override VarType ReturnType() => (Resolved.Type as VarTypeFunction).ReturnType.GetVarType();

    public override void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder) {
        if (Resolved.Inline) Resolved.CompileDeclarations(mod, builder);
    }

    public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        Resolved.AddToCompileTimeScope(param, param.RootScope.EnterScope(BestFit.Name, false), TemplateParameters);
        LLVMValueRef[] args = new LLVMValueRef[Args.Length];
        for (int i = 0; i < args.Length; i++) {
            args[i] = Args[i].CompileRValue(mod, builder, param);
        }
        var callee = Resolved.Compile(mod, builder, param);
        if (callee != null) {

            // Normal call.
            return builder.BuildCall(Resolved.Compile(mod, builder, param), args);

        } else {

            // Inline call.
            return Resolved.CompileInline(mod, builder, param, args);

        }
    }

    public override string ToString() {
        string ret = "(" + ReturnType().ToString() + ")Operator(" + Op;
        foreach (var a in Args) {
            ret += a.ToString();
            if (a != Args.Last()) ret += ", ";
        }
        return ret + ")";
    }

}