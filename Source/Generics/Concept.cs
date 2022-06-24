using WARD.Types;

namespace WARD.Generics;

// A concept is a way of grouping types together. Any concept can be created by types and concepts.
public abstract partial class Concept {

    // If a given type fits a concept.
    public abstract bool TypeFitsConcept(VarType type);

}