using WARD.Statements;

namespace WARD.Common;

// A collection of definitions to be compiled.
public class CompilationUnit {
    public List<ICompileableTopLevel> Items = new List<ICompileableTopLevel>(); // Items to compile.
}