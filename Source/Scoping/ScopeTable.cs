using WARD.Common;
using WARD.Exceptions;
using WARD.Statements;
using WARD.Types;

namespace WARD.Scoping;

// Table for resolving types and variables.
public class ScopeTable {
    private Scope Scope = null; // Scope containing this table.
    private Dictionary<string, VarType> Types = new Dictionary<string, VarType>(); // Types that are available.
    private Dictionary<string, Variable> Variables = new Dictionary<string, Variable>(); // Variables that are available.
    private List<Function> Functions = new List<Function>(); // Functions that are available.
    private List<FunctionGeneric> GenericFunctions = new List<FunctionGeneric>(); // Subset of functions that are also generic.
    private Dictionary<string, List<FunctionGeneric>> Operators = new Dictionary<string, List<FunctionGeneric>>(); // Operators that are available.

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
        var genericFunc = function as FunctionGeneric;
        if (addVariable && genericFunc == null) AddVariable(function); // Generic functions don't have an actual address.
        if (genericFunc != null) {
            bool alreadyDefined = false;
            foreach (var g in GenericFunctions) { // Check for function being defined already.
                alreadyDefined |= !(g.FuncName.Equals(genericFunc.FuncName) && g.Template.Equals(genericFunc.Template));
            }
            if (alreadyDefined) {
                Error.ThrowInternal("Duplicate template for generic function \"" + function.FuncName + "\".");
            } else {
                GenericFunctions.Add(genericFunc);
            }
        }
    }

    // Add an operator to the scope table.
    public void AddOperator(string op, FunctionGeneric function, bool addFunction = true) {
        if (!Operators.ContainsKey(op)) {
            Operators.Add(op, new List<FunctionGeneric>());
        }
        var list = Operators[op];
        foreach (var func in list) {
            if (func.Template.Equals(function.Template)) {
                Error.ThrowInternal("Duplicate template for generic operator function \"" + function.FuncName + "\" for operator \"" + op + "\".");
                return;
            }
        }
        if (addFunction) AddFunction(function);
        list.Add(function);
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
            AddFunction(f, false); // Takes care of generics too.
        }
        foreach (var o in table.Operators) {
            foreach (var f in o.Value) {
                AddOperator(o.Key, f, false);
            }
        }
    }

    // Find a scope table that matches the base path.
    public ScopeTable FindWithMatchingBasepath(string path) {

        // No base path.
        if (!path.Contains(".")) return this;

        // Find with base path.
        string basePath = path.Substring(0, path.LastIndexOf("."));
        Scope currScope = Scope;
        while (!currScope.ScopeExists(basePath)) {
            if (currScope.Parent == null) return null;
            currScope = currScope.Parent;
        }
        return currScope.Table;

    }

    // Get the name of an item.
    public string GetItemName(string path) {
        if (path.Contains(".")) {
            return path.Split(".").Last();
        } else {
            return path;
        }
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

    // Resolve functions that match an operator.
    public List<FunctionGeneric> ResolveOperator(string op) {
        List<FunctionGeneric> ret = new List<FunctionGeneric>();
        if (Operators.ContainsKey(op)) ret.AddRange(Operators[op]);
        if (Scope.Parent != null) ret.AddRange(Scope.Parent.Table.ResolveOperator(op));
        return ret;
    }

}