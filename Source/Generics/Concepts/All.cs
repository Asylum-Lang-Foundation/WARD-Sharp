using WARD.Types;

namespace WARD.Generics;

// Represents a concept for all types.
public class ConceptAll : Concept {
    public override bool TypeFitsConcept(VarType type) {
        return true;
    }
}