using WARD.Statements;

namespace WARD.Builder;

// Some statements that can just be added.
public partial class CodeBuilder {

    // Add a statement to the code.
    public void Code(ICompileable statement) {
        Statements.Add(statement);
    }

}