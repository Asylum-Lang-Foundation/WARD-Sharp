using WARD.Exceptions;
using WARD.Generics;
using WARD.Scoping;
using WARD.Statements;

namespace WARD.Builder;

// Function operations.
public partial class UnitBuilder {

    // Add a function.
    public void AddFunction(Function function, Tuple<CodeStatements, Scope> definition = null) {
        if (definition != null) {
            function.Definition = definition.Item1;
            CurrentScope.EnterScope(function.Name).ImportScope(definition.Item2);
        }
        Unit.Items.Add(function);
        CurrentScope.Table.AddFunction(function);
    }

    // Add an operator.
    public void AddOperator(Tuple<string, FunctionGeneric, Tuple<CodeStatements, Scope>> operatorDef) {

        // Make sure that the function template is defined by the arguments only.
        if (!operatorDef.Item2.ImplicitTemplateInitializationPossible()) {
            Error.ThrowInternal("Custom operator \"" + operatorDef.Item2.FuncName + "\" can only have type parameters dependent solely on its input arguments.");
            return;
        }
        operatorDef.Item2.Definition = operatorDef.Item3.Item1;
        CurrentScope.EnterScope(operatorDef.Item2.Name).ImportScope(operatorDef.Item3.Item2);
        CurrentScope.Table.AddOperator(operatorDef.Item1, operatorDef.Item2);

    }

    // Add a cast. TODO!!!

}