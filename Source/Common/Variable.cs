using WARD.Types;

namespace WARD.Common;

// Contains information about a variable. TODO: MANGLING FOR FUNCTIONS!!!
public class Variable {
    public string Name { get; internal set; } // Name of the variable.
    public VarType Type { get; } // Type of the variable.
    public DataAccessFlags AccessFlags { get; } // How the data is accessed.

    // Create a new variable. WARNING: This does not append it to the scope table!
    public Variable(string name, VarType type, DataAccessFlags accessFlags = DataAccessFlags.Read | DataAccessFlags.Write) {
        Name = name;
        Type = type;
        AccessFlags = accessFlags;
    }

}