using WARD.Types;

namespace WARD.Generics;

// Represents a concept for no types.
public class ConceptNone : Concept {
    public override bool TypeFitsConcept(VarType type) {
        return false;
    }
}