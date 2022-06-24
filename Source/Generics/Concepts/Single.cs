using WARD.Types;

namespace WARD.Generics;

// Represents a concept for a single type.
public class ConceptSingle : Concept {
    private VarType Type; // The type that is allowed.

    // Type to accept.
    public ConceptSingle(VarType type) {
        Type = type;
    }

    public override bool TypeFitsConcept(VarType type) {
        return Type.Equals(type);
    }

}