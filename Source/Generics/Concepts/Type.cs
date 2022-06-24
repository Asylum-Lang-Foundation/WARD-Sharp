using WARD.Types;

namespace WARD.Generics;

// Represents a concept for a particular type.
public class ConceptType : Concept {
    private VarTypeEnum Type; // The type that is allowed.

    // Type to accept.
    public ConceptType(VarTypeEnum type) {
        Type = type;
    }

    public override bool TypeFitsConcept(VarType type) {
        return type.Type == Type;
    }

}