using WARD.Statements;

namespace WARD.Builder;

// Function operations.
public partial class UnitBuilder {

    // Add a function.
    public void AddFunction(Function function, CodeStatements definition) {
        function.Definition = definition;
        throw new System.NotImplementedException();
    }

}