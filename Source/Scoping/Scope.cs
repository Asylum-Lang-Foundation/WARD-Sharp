using WARD.Exceptions;

namespace WARD.Scoping;

// A scope for items to be present in.
public class Scope {
    public ScopeTable Table { get; } // Scope table.
    public Scope Parent { get; } = null; // Parent scope.
    private Dictionary<string, Scope> Children = new Dictionary<string, Scope>(); // Child scopes.

    // Create a new scope.
    public Scope(string name = "", Scope parent = null) {
        Table = new ScopeTable(this);
        Parent = parent;
        if (Parent != null) Parent.Children.Add(name, this);
    }

    // Get full path of scope.
    public string GetFullPath() {
        if (Parent == null) return "";
        return Parent.GetFullPath() + "." + Parent.Children.Where(x => x.Value == this).ElementAt(0).Key;
    }

    // Enter a scope.
    public Scope EnterScope(string path, bool addIfNotExistant = true) {
        if (path.Equals("")) return this;
        string toEnter = path.Split(".")[0];
        if (!Children.ContainsKey(toEnter)) {
            if (addIfNotExistant) {
                new Scope(toEnter, this);
            } else {
                Error.ThrowInternal("Scope \"" + GetFullPath() + "." + path + "\" does not exist.");
                return null;
            }
        }
        if (path.Contains(".")) {
            return Children[toEnter].EnterScope(path.Substring(path.IndexOf(".") + 1));
        } else {
            return Children[toEnter];
        }
    }

    // Exit a scope.
    public Scope ExitScope() {
        if (Parent == null) {
            Error.ThrowInternal("The root scope has no parent scope.");
        }
        return Parent;
    }

    // Remove a scope.
    public void Remove(string path, bool failIfNonExistant = true) {
        if (failIfNonExistant && path.Equals("")) {
            Error.ThrowInternal("Can not remove a scope you are currently in, \"" + GetFullPath() + "\".");
            return;
        }
        if (path.Contains(".")) {
            EnterScope(path.Substring(0, path.IndexOf("."))).Remove(path.Substring(path.IndexOf(".") + 1));
        } else {
            if (Children.ContainsKey(path)) {
                Children.Remove(path);
            } else {
                Error.ThrowInternal("Can not remove scope \"" + GetFullPath() + "." + path + "\", as it does not exist.");
            }
        }
    }

    // Import a scope.
    public void ImportScope(Scope toImport) {
        Table.Merge(toImport.Table);
        foreach (var c in toImport.Children) {
            EnterScope(c.Key).ImportScope(c.Value);
        }
    }

}