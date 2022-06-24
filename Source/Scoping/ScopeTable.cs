using WARD.Common;
using WARD.Exceptions;
using WARD.Statements;
using WARD.Types;

namespace WARD.Scoping;

// Table for resolving types and variables.
public class ScopeTable {
    private Scope Scope = null; // Scope containing this table.
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

    // Find a scope table that matches the base path.
    public ScopeTable FindWithMatchingBasepath(string path) {
        throw new System.NotImplementedException();
    }

    // Get the name of an item.
    public string GetItemName(string path) {
        throw new System.NotImplementedException();
    }

    // Resolve a variable.
    public Variable ResolveVariable(string path) {

        // Split base path and name.
        ScopeTable match = FindWithMatchingBasepath(path);
        string varName = GetItemName(path);
        if (match == null) return null;

        // Variable found, all is well.
        if (match.Variables.ContainsKey(varName)) {
            return match.Variables[varName];
        }

        // Variable not found, search parent if possible.
        else {
            if (match.Scope.Parent != null) {
                return match.Scope.Parent.Table.ResolveVariable(path);
            } else {
                return null;
            }
        }

    }

    // Resolve possible functions.
    public List<Function> ResolveFunctions(string path) {

        // Prepare list of functions and separate function name from path.
        List<Function> funcs = new List<Function>();
        ScopeTable match = FindWithMatchingBasepath(path);
        string funcName = GetItemName(path);
        if (match == null) return funcs;

        // Add matching functions and search parent of match if applicable.
        funcs.AddRange(match.Functions.Where(x => x.FuncName.Equals(funcName)));
        if (match.Scope.Parent != null) {
            funcs.AddRange(match.Scope.Parent.Table.ResolveFunctions(path));
        }
        return funcs;

    }

}