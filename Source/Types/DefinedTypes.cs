namespace WARD.Types;

// Predefined types.
public partial class VarType {

    // Standard sizes.
    public static VarType Int8_t { get; } = new VarTypeInteger(true, 8);
    public static VarType UInt8_t { get; } = new VarTypeInteger(false, 8);
    public static VarType Int16_t { get; } = new VarTypeInteger(true, 16);
    public static VarType UInt16_t { get; } = new VarTypeInteger(false, 16);
    public static VarType Int32_t { get; } = new VarTypeInteger(true, 32);
    public static VarType UInt32_t { get; } = new VarTypeInteger(false, 32);
    public static VarType Int64_t { get; } = new VarTypeInteger(true, 64);
    public static VarType UInt64_t { get; } = new VarTypeInteger(false, 64);

    // Typedefs.
    public static VarType SByte { get; } = Int8_t;
    public static VarType Byte { get; } = UInt8_t;
    public static VarType Short { get; } = Int16_t;
    public static VarType UShort { get; } = UInt16_t;
    public static VarType Int { get; } = Int32_t;
    public static VarType UInt { get; } = UInt32_t;
    public static VarType Long { get; } = Int64_t;
    public static VarType ULong { get; } = UInt64_t;

    // Alternate versions.
    public static VarType Char { get; } = SByte;
    public static VarType Word { get; } = UShort;
    public static VarType DWord { get; } = UInt;
    public static VarType QWord { get; } = ULong;

}