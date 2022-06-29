using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Expressions;

namespace WARD.Types;

// Alias to another type. TODO: FIGURE OUT HOW TO RESOLVE A TYPE!
public class VarTypeAlias : VarType {
    //private VarType Resolved; // The resolved variable type.
    public string Alias { get; } // Alias the type is referring to.
    private VarType Resolved; // Resolved type.

    // Resolve a type alias in the current scope.
    public VarTypeAlias(string alias) {
        Type = VarTypeEnum.Alias;
        Alias = alias;
    }

    public override VarType GetVarType() {
        if (Resolved != null) return Resolved.GetVarType();
        Error.ThrowInternal("Type alias \"" + Alias + "\" has not been resolved.");
        return null;
    }

    protected override LLVMTypeRef LLVMType() {
        if (Resolved != null) return Resolved.GetLLVMType();
        Error.ThrowInternal("Type alias \"" + Alias + "\" has not been resolved.");
        return null;
    }

    public override string Mangled() => Alias.Length + Alias + "E";

    public override bool Equals (object other) {
        if (Resolved != null) return Resolved.Equals(other); // Resolution was done, do proper type checking.
        VarTypeAlias o = other as VarTypeAlias;
        if (o != null) {
            return Alias.Equals(o.Alias); // Since type resolution is deferred, can't really do much at this stage.
        }
        return false;
    }

    public override int GetHashCode() {
        if (Resolved != null) return Resolved.GetHashCode();
        HashCode ret = new HashCode();
        ret.Add(Type);
        ret.Add(Alias);
        return ret.ToHashCode();
    }

    public override string ToString() => Alias;

    public override Expression DefaultValue() {
        if (Resolved != null) return Resolved.DefaultValue();
        Error.ThrowInternal("Type alias \"" + Alias + "\" has not been resolved.");
        return null;
    }

}