using System.Diagnostics;
using System.Runtime.InteropServices;
using WARD.Builder;
using WARD.Common;
using WARD.Expressions;
using WARD.Operators;
using WARD.Runtime;
using WARD.Statements;
using WARD.Types;

// Setup builders.
ProgramBuilder pb = new ProgramBuilder();
pb.RegisterStandardOperators();
UnitBuilder ub = pb.NewCompilationUnit("Addition");

// Addition function.
CodeBuilder cb = new CodeBuilder();
cb.Code(new StatementReturn(new ExpressionOperator("Add", new ExpressionVariable("num1"), new ExpressionVariable("num2"))));
var addFunc = new Function(
    "add",
    new VarTypeFunction(VarType.Int, null, new Variable("num1", VarType.Int), new Variable("num2", VarType.Int))
);
ub.AddFunction(addFunc, cb.EndBuilding());

// Finish the unit and compile.
pb.EndCompilationUnit(ub);
CompiledProgram program = pb.Compile();

// Sample of how to export a compilation unit to various output formats.
program.ExportLLVMAssembly("Addition", "Addition.ll");
program.ExportLLVMBitcode("Addition", "Addition.bc");
program.ExportAssembly("Addition", "Addition.s");
program.ExportObject("Addition", "Addition.o");

// Execute the addition function and test that it works.
BinaryInt32Operation addOp = program.GetFunctionExecuter<BinaryInt32Operation>(addFunc);
int result = addOp(3, 5);
Console.WriteLine(result);
Trace.Assert(result == 8);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate int BinaryInt32Operation(int op1, int op2);