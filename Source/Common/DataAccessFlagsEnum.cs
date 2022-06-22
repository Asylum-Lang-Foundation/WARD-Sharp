namespace WARD.Common;

// Dictate what is allowed to be done to a variable/memory location.
[Flags]
public enum DataAccessFlags : int {
    Read = 0b1 << 0, // Ability to read from the memory location.
    Write = 0b1 << 1, // Ability to write to the memory location.
    Static = 0b1 << 2, // The memory location is in static memory and is only in one place in the program.
    Atomic = 0b1 << 3, // Memory can only be modified by one flag at a time.
    Volatile = 0b1 << 4, // A read and write must be done for every access.
}