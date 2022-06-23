namespace WARD.Generics;

// Define some useful concepts.
public abstract partial class Concept {
    public static Concept WholeNumber { get; } = new ConceptType(Types.VarTypeEnum.Integer);
    public static Concept DecimalNumber { get; } = new ConceptUnion(new ConceptType(Types.VarTypeEnum.Fixed), new ConceptType(Types.VarTypeEnum.Floating));
    public static Concept Number { get; } = new ConceptUnion(WholeNumber, DecimalNumber);
}