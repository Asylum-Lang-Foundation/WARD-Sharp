using WARD.Expressions;

namespace WARD.Common;

// Attributes that can determine certain properties of functions.
public class ItemAttribute {
    public string Name { get; } // Name of the attribute.
    public Expression Expression { get; } // Expression to evaluate.

    // Create a new item attribute.
    public ItemAttribute(string name, Expression expression = null) {
        Name = name;
        Expression = expression;
    }

}