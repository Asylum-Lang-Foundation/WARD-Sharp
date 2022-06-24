using WARD.Common;
using WARD.Generics;
using WARD.Types;

namespace WARD.Statements;

// Create a function. TODO: EVERYTHING!!!
public class FunctionGeneric : Function {
    public Template Template { get; } // Template that the function uses.

    // Create a new generic function. WARNING: This does not add the function to the scope (do it manually to add both the mangled variable name and regular function name).
    public FunctionGeneric(string name, VarTypeFunction signature, Template template, params ItemAttribute[] attributes) : base(name, signature, attributes) {
        Template = template;
    }

}