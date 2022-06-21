using LLVMSharp.Interop;
using WARD.Exceptions;
using WARD.Expressions;

namespace WARD.Types;

// Simple type.
public class VarTypeSimple : VarType {
    public VarTypeSimpleEnum SimpleType { get; } // Type of simple primitive to compile.

    // Create a new simple primitive.
    public VarTypeSimple(VarTypeSimpleEnum simpleType) {
        Type = VarTypeEnum.Simple;
        SimpleType = simpleType;
    }

    public override VarType GetVarType() => this;
    protected override LLVMTypeRef LLVMType() {
        switch (SimpleType) {
            case VarTypeSimpleEnum.Void: return LLVMTypeRef.Void;
            case VarTypeSimpleEnum.Object: return LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
            default:
                Error.ThrowInternal("Unknown simple primitive variable type.");
                return null;
        }
    }
    public override string Mangled() {
        switch (SimpleType) {
            case VarTypeSimpleEnum.Void: return "v";
            case VarTypeSimpleEnum.Object: return "o";
            default:
                Error.ThrowInternal("Unknown simple primitive variable type.");
                return null;
        }
    }

    public override bool Equals (object other) {
        var o = other as VarTypeSimple;
        if (o != null) {
            return SimpleType == o.SimpleType;
        }
        return false;
    }

    public override int GetHashCode() {
        HashCode ret = new HashCode();
        ret.Add(Type);
        ret.Add(SimpleType);
        return ret.ToHashCode();
    }

    public override string ToString() {
        switch (SimpleType) {
            case VarTypeSimpleEnum.Void: return "void";
            case VarTypeSimpleEnum.Object: return "object";
            default:
                Error.ThrowInternal("Unknown simple primitive variable type.");
                return null;
        }
    }

    public override Expression DefaultValue() {
        Error.ThrowInternal("A simple type can not have a default value.");
        return null;
    }

}