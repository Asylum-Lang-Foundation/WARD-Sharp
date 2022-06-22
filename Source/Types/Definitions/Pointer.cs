using LLVMSharp.Interop;
using WARD.Expressions;

namespace WARD.Types;

// Pointer type.
public class VarTypePointer : VarType {
    public VarType PointedTo { get; } // Type that is pointed to.

    // Null pointer value.
    public ExpressionConstPointer NullPointer => ExpressionConstPointer.NullPointer(this);

    // Create a new pointer type.
    public VarTypePointer(VarType pointedTo) {
        Type = VarTypeEnum.Pointer;
        PointedTo = pointedTo;
    }

    public override VarType GetVarType() => this;
    protected override LLVMTypeRef LLVMType() => LLVMTypeRef.CreatePointer(PointedTo.GetLLVMType(), 0);
    public override string Mangled() => "p" + PointedTo.Mangled();

    public override bool Equals (object other) {
        var o = other as VarTypePointer;
        if (o != null) {
            return PointedTo.Equals(o.PointedTo);
        }
        return false;
    }

    public override int GetHashCode() {
        HashCode ret = new HashCode();
        ret.Add(Type);
        ret.Add(PointedTo);
        return ret.ToHashCode();
    }

    public override string ToString() {
        return PointedTo.ToString() + "*";
    }

    public override Expression DefaultValue() => NullPointer;

}