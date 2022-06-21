using LLVMSharp.Interop;
using WARD.Common;
using WARD.Expressions;

namespace WARD.Types;

// Function type. TODO!!!
public class VarTypeFunction : VarType {
    public VarType ReturnType { get; } // What the function returns.
    public Variable VariadicType { get; } // If the function supports infinite arguments of a type.
    public Variable[] Parameters { get; } // Parameters for the function.

    // Create a new integer (signed for negative values, bitwidth for how many bits including signed bit).
    public VarTypeFunction(VarType returnType, Variable variadicType, params Variable[] parameters) {
        Type = VarTypeEnum.Function;
        ReturnType = returnType;
        VariadicType = variadicType;
        Parameters = parameters;
    }

    public override VarType GetVarType() => this;
    protected override LLVMTypeRef LLVMType() => throw new System.NotImplementedException(); //LLVMTypeRef.CreateFunction(ReturnType.GetLLVMType(), , VariadicType != null,);
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