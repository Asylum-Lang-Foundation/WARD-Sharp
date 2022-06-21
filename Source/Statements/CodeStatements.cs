using LLVMSharp.Interop;
using WARD.Common;
using WARD.Exceptions;

namespace WARD.Statements;

// A collection of statements that can be compiled.
public class CodeStatements : ICompileable {
    private bool BlockTerminated; // If the block has been terminated or not.
    private LLVMValueRef ReturnedValue; // Value for this block to return.
    public List<ICompileable> Statements { get; } // Statements to compile.
    public FileContext FileContext; // Context for the statements.

    public FileContext GetFileContext() => FileContext;

    // Create new code statements.
    public CodeStatements(params ICompileable[] statements) {
        Statements = statements.ToList();
        BlockTerminated = false;
        ReturnedValue = null;
    }

    // Add a code statement.
    public void Add(params ICompileable[] statement) {
        Statements.AddRange(statement);
    }

    // Terminate future code statements.
    public void TerminateBlock() {
        if (BlockTerminated) {
            Error.ThrowInternal("Block has already been terminated.");
            return;
        }
        BlockTerminated = true;
    }

    // Allow block to continue.
    public void BeginBlock() {
        BlockTerminated = false;
    }

    // Return a value.
    public void ReturnAValue(LLVMValueRef val) {
        TerminateBlock();
        ReturnedValue = val;
    }

    public void ResolveVariables() {
        foreach (var c in Statements) {
            c.ResolveVariables();
        }
    }

    public void ResolveTypes() {
        foreach (var c in Statements) {
            c.ResolveTypes();
        }
    }

    public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder) {
        foreach (var s in Statements) {
            s.CompileDeclarations(mod, builder);
        }
    }

    public LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, CompilationContext param) {
        foreach (var s in Statements) {
            if (!BlockTerminated) {
                param.CodeStatementsStack.Push(this);
                s.Compile(mod, builder, param);
                param.CodeStatementsStack.Pop();
            }
        }
        return ReturnedValue;
    }

    public override string ToString() {
        string ret = "{\n";
        foreach (var c in Statements) {
            ret += c.ToString() + "\n";
        }
        return ret + "}";
    }

}