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
        throw new System.NotImplementedException();
    }

    // Add a cast. TODO!!!

}