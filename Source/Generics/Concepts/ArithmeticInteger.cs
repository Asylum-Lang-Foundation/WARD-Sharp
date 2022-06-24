using WARD.Types;

namespace WARD.Generics;

// Represents a concept for whole numbers that you can do math on.
public class ConceptArithmeticInteger : Concept {
    public override bool TypeFitsConcept(VarType type) {
        var t = type as VarTypeInteger;
        if (t != null) {
            int intBase = (int)Math.Log2(VarType.Int.BitWidth);
            double currIntBase = Math.Log2(t.BitWidth);
            double rounded = Math.Round(currIntBase);
            if (Math.Abs(rounded - currIntBase) < 0.0000001) { // Must be clean log.
                return (int)rounded >= currIntBase;
            }
        }
        return false;
    }
}