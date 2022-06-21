using WARD.Statements;

namespace WARD.Builder;

// For building code statements.
public partial class CodeBuilder {
    internal CodeStatements Statements = new CodeStatements(); // Statements to append to.

    // Finish building and return the code statements.
    public CodeStatements EndBuilding() {
        return Statements;
    }

}