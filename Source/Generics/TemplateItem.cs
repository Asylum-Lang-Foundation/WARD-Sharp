namespace WARD.Generics;

// An item for templates to use.
public class TemplateItem {
    public string Name { get; } // Name of the template item.
    public TemplateItemTypeEnum Type { get; } // What type to classify the item as.
    public Concept Concept { get; } // Concept the item must follow.

    // Create a new template item.
    public TemplateItem(string name, TemplateItemTypeEnum type, Concept concept) {
        Name = name;
        Type = type;
        Concept = concept;
    }

}