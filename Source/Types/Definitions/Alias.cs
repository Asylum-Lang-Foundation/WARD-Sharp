using LLVMSharp.Interop;
using WARD.Expressions;

namespace WARD.Types;

// Alias to another type. TODO: FIGURE OUT HOW TO RESOLVE A TYPE!
public class VarTypeAlias : VarType {
    private VarType Resolved; // The resolved variable type.
    public string Alias { get; } // Alias the type is referring to.

    // Resolve a type alias in the current scope.
    public VarTypeAlias(string alias) {
        Type = VarTypeEnum.Alias;
        Alias = alias;
    }

    public override VarType GetVarType() => throw new System.NotImplementedException();
    protected override LLVMTypeRef LLVMType() => throw new System.NotImplementedException();
    public override string Mangled() => throw new System.NotImplementedException();

    public override bool Equals (object other) {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode() {
        throw new System.NotImplementedException();
    }

    public override string ToString() {
        throw new System.NotImplementedException();
    }

    public override Expression DefaultValue() => throw new System.NotImplementedException();

}