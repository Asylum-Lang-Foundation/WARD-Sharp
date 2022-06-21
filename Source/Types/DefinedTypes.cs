namespace WARD.Types;

// Predefined types.
public partial class VarType {

    // Simple types.
    public static VarType Void { get; } = new VarTypeSimple(VarTypeSimpleEnum.Void);
    public static VarType Object { get; } = new VarTypeSimple(VarTypeSimpleEnum.Object);

    // Standard sizes.
    public static VarTypeInteger Int8_t { get; } = new VarTypeInteger(true, 8);
    public static VarTypeInteger UInt8_t { get; } = new VarTypeInteger(false, 8);
    public static VarTypeInteger Int16_t { get; } = new VarTypeInteger(true, 16);
    public static VarTypeInteger UInt16_t { get; } = new VarTypeInteger(false, 16);
    public static VarTypeInteger Int32_t { get; } = new VarTypeInteger(true, 32);
    public static VarTypeInteger UInt32_t { get; } = new VarTypeInteger(false, 32);
    public static VarTypeInteger Int64_t { get; } = new VarTypeInteger(true, 64);
    public static VarTypeInteger UInt64_t { get; } = new VarTypeInteger(false, 64);

    // Typedefs.
    public static VarTypeInteger SByte { get; } = Int8_t;
    public static VarTypeInteger Byte { get; } = UInt8_t;
    public static VarTypeInteger Short { get; } = Int16_t;
    public static VarTypeInteger UShort { get; } = UInt16_t;
    public static VarTypeInteger Int { get; } = Int32_t;
    public static VarTypeInteger UInt { get; } = UInt32_t;
    public static VarTypeInteger Long { get; } = Int64_t;
    public static VarTypeInteger ULong { get; } = UInt64_t;

    // Alternate versions.
    public static VarTypeInteger Char { get; } = SByte;
    public static VarTypeInteger Word { get; } = UShort;
    public static VarTypeInteger DWord { get; } = UInt;
    public static VarTypeInteger QWord { get; } = ULong;

}