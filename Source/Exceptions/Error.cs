namespace WARD.Exceptions;

// For throwing errors.
public static class Error {

    // Throw an error internally.
    public static void ThrowInternal(string msg) {
        throw new Exception("COMPILER ERROR: " + msg);
    }

}