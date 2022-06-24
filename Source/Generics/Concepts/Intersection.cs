using WARD.Types;

namespace WARD.Generics;

// Represents a concept that is an intersection of existing ones.
public class ConceptIntersection : Concept {
    private Concept[] Concepts; // Concepts to modify.

    // Take the intersection of concepts.
    public ConceptIntersection(params Concept[] concepts) {
        Concepts = concepts;
    }

    public override bool TypeFitsConcept(VarType type) {
        foreach (var c in Concepts) {
            if (!c.TypeFitsConcept(type)) return false;
        }
        return true;
    }

}