using WARD.Types;

namespace WARD.Generics;

// Represents a concept that is the compliment of an existing one.
public class ConceptCompliment : Concept {
    private Concept Concept; // Concept to modify.

    // Take the opposite of a concept.
    public ConceptCompliment(Concept concept) {
        Concept = concept;
    }

    public override bool TypeFitsConcept(VarType type) {
        return !Concept.TypeFitsConcept(type);
    }

}