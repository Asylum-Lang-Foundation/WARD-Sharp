using LLVMSharp.Interop;
using WARD.Expressions;

namespace WARD.Types;

// Integer type.
public class VarTypeInteger : VarType {
    public bool Signed { get; } // If the integer is signed or unsigned.
    public uint BitWidth { get; } // How many bits the integer has.

    // Create a new integer (signed for negative values, bitwidth for how many bits including signed bit).
    public VarTypeInteger(bool signed, uint bitWidth) {
        Type = VarTypeEnum.Integer;
        Signed = signed;
        BitWidth = bitWidth;
    }

    public override VarType GetVarType() => this;
    protected override LLVMTypeRef LLVMType() => LLVMTypeRef.CreateInt(BitWidth);
    public override string Mangled() => (Signed ? "s" : "u") + BitWidth.ToString() + "E";

    public override bool Equals (object other) {
        var o = other as VarTypeInteger;
        if (o != null) {
            return Signed == o.Signed && BitWidth == o.BitWidth;
        }
        return false;
    }

    public override int GetHashCode() {
        HashCode ret = new HashCode();
        ret.Add(Type);
        ret.Add(Signed);
        ret.Add(BitWidth);
        return ret.ToHashCode();
    }

    public override string ToString() {
        return (Signed ? "s" : "u") + BitWidth.ToString();
    }

    public override Expression DefaultValue() => new ExpressionConstInt(Signed, BitWidth, 0);

}