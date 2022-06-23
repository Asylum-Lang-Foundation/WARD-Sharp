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

    // Add a generic function.
    public void AddGenericFunction(Function function, Template template, Tuple<CodeStatements, Scope> definition = null) {
        throw new System.NotImplementedException();
    }

}