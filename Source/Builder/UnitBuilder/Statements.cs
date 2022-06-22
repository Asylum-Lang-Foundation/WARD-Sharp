using WARD.Exceptions;
using WARD.Statements;

namespace WARD.Builder;

// Append statements to a unit.
public partial class UnitBuilder {
    private CodeStatements CurrentStatements = null; // Current statements that are being modified.

    // Add a code statement.
    public void Code(ICompileable statement) {
        if (CurrentStatements == null) {
            Error.ThrowInternal("Can not add code outside of a function.");
            return;
        }
        CurrentStatements.Add(statement);
    }

    // TODO: MAKE THIS A CODE BUILDER THING!!!

}