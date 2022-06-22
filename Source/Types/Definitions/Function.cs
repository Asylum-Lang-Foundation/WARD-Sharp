using LLVMSharp.Interop;
using WARD.Common;
using WARD.Exceptions;
using WARD.Expressions;

namespace WARD.Types;

// Function type.
public class VarTypeFunction : VarType {
    public VarType ReturnType { get; } // What the function returns.
    public Variable VariadicType { get; } // If the function supports infinite arguments of a type.
    public Variable[] Parameters { get; } // Parameters for the function.

    // Create a new function type signature (only types of parameters are used).
    public VarTypeFunction(VarType returnType, Variable variadicType = null, params Variable[] parameters) {
        Type = VarTypeEnum.Function;
        ReturnType = returnType;
        VariadicType = variadicType;
        Parameters = parameters;
    }

    public override VarType GetVarType() => this;
    protected override LLVMTypeRef LLVMType() => LLVMTypeRef.CreateFunction(ReturnType.GetLLVMType(), Parameters.Select(x => x.Type.GetLLVMType()).ToArray(), VariadicType != null);
    public override string Mangled() {
        string ret = "";
        foreach (var p in Parameters) {
            ret += p.Type.Mangled();
        }
        if (VariadicType != null) ret += VariadicType.Type.Mangled() + "I";
        if (!ReturnType.Equals(VarType.Void)) ret += "R" + ReturnType.Mangled();
        return ret;
    }

    protected List<VarType> GetTypeList() {
        List<VarType> ret = new List<VarType>();
        ret.Add(ReturnType);
        ret.Add(VariadicType != null ? VariadicType.Type : null);
        foreach (var p in Parameters) {
            ret.Add(p.Type);
        }
        return ret;
    }

    public override bool Equals (object other) {
        var o = other as VarTypeFunction;
        if (o != null) {
            List<VarType> typeList = GetTypeList();
            List<VarType> typeListNew = o.GetTypeList();
            if (typeList.Count != typeListNew.Count) return false;
            for (int i = 0; i < typeList.Count; i++) {
                if (!typeList[i].Equals(typeListNew[i])) return false;
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode() {
        return GetTypeList().GetHashCode();
    }

    public override string ToString() {
        string ret = "func<" + ReturnType.ToString();
        foreach (var p in Parameters) {
            ret += ", " + p.Type.ToString();
        }
        if (VariadicType != null) ret += ", " + VariadicType.Type.ToString() + "...";
        return ret + ">";
    }

    public override Expression DefaultValue() {
        Error.ThrowInternal("A function type can not have a default value.");
        return null;
    }

}