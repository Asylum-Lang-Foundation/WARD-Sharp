using WARD.Common;
using WARD.Exceptions;
using WARD.Statements;
using WARD.Types;

namespace WARD.Scoping;

// Table for resolving types and variables.
public class ScopeTable {
    private Scope Scope = null;
    private Dictionary<string, VarType> Types = new Dictionary<string, VarType>(); // Types that are available.
    private List<Function> Functions = new List<Function>(); // Functions that are available.
    private Dictionary<string, Variable> Variables = new Dictionary<string, Variable>(); // Variables that are available.

    // Create a new scope table.
    public ScopeTable(Scope scope) {
        Scope = scope;
    }

    // Add a type to the scope table.
    public void AddType(string name, VarType type) {
        if (Types.ContainsKey(name)) {
            Error.ThrowInternal("Type \"" + Scope.GetFullPath() + "." + name + "\" already exists.");
            return;
        }
        Types.Add(name, type);
    }

    // Add a variable to the scope table.
    public void AddVariable(Variable variable) {
        if (Variables.ContainsKey(variable.Name)) {
            Error.ThrowInternal("Variable \"" + Scope.GetFullPath() + "." + variable.Name + "\" already exists.");
            return;
        }
        Variables.Add(variable.Name, variable);
    }

    // Add a function to the scope table. A function is a variable, but the variable version has the mangled name and so should be unique while the function name isn't.
    public void AddFunction(Function function, bool addVariable = true) {
        Functions.Add(function);
        if (addVariable) AddVariable(function);
    }

    // Merge with another table.
    public void Merge(ScopeTable table) {
        foreach (var t in table.Types) {
            AddType(t.Key, t.Value);
        }
        foreach (var v in table.Variables) {
            AddVariable(v.Value);
        }
        foreach (var f in table.Functions) {
            AddFunction(f, false);
        }
    }

}