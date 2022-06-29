using WARD.Operators;
using WARD.Scoping;
using WARD.Statements;

namespace WARD.Builder;

// For defining operators.
public partial class ProgramBuilder {
    private UnitBuilder WARDBuilder = new UnitBuilder("__WARD_INTERNAL__");

    // Add in all the standard operators.
    public void RegisterStandardOperators() {
        WARDBuilder.AddOperator(Operator.IntegerAddition());
    }

}