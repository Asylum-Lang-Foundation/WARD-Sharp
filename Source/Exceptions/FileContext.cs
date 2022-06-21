namespace WARD.Exceptions;

// File context.
public class FileContext {
    public string Context; // Line with an error.
    public string FileName; // File that the error is from.
    public int LineNum; // Line number the error is from.
    public int ColumnNum; // Column where to find the error.
}