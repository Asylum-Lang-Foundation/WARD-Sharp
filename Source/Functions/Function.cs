using WARD.Common;
using WARD.Types;

namespace WARD.Functions;

// Create a function. TODO: FUNCTION FOR DECLARING DEFINTION!!!
public class Function : Variable {
    public VarTypeFunction Signature { get; }

    // Create a new function. WARNING: This does not add the function to the scope (do it manually to add both the mangled variable name and regular function name).
    public Function(string name, VarTypeFunction signature) : base(signature.Mangled(), signature, DataAccessFlags.Read | DataAccessFlags.Static) {
        Signature = signature;
    }

}