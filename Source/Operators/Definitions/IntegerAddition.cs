using WARD.Builder;
using WARD.Common;
using WARD.Expressions;
using WARD.Generics;
using WARD.Scoping;
using WARD.Statements;
using WARD.Types;

namespace WARD.Operators;

// Addition operator.
public static partial class Operators {

    // Addition operator for whole numbers.
    public static Tuple<FunctionGeneric, Tuple<CodeStatements, Scope>> IntegerAddition() {

        // Define template.
        var template = new Template(new TemplateItem("T", TemplateItemTypeEnum.Type, Concept.ArithmeticInteger));

        // Define the function body.
        var cb = new CodeBuilder();
        cb.Code(new StatementReturn(
            new ExpressionLLVM(
                "add",
                new VarTypeAlias("T"),
                new ExpressionVariable("num1"),
                new ExpressionVariable("num2")
            )
        ));

        // Define the function.
        FunctionGeneric function = new FunctionGeneric(
            "W_BuiltInOperator_IntegerAddition",
            new VarTypeFunction(
                new VarTypeAlias("T"),
                null,
                new Variable("num1", new VarTypeAlias("T")),
                new Variable("num2", new VarTypeAlias("T"))
            ),
            template,
            new ItemAttribute("Inline")
        );

        // Return data.
        return new Tuple<FunctionGeneric, Tuple<CodeStatements, Scope>>(function, cb.EndBuilding());

    }

}