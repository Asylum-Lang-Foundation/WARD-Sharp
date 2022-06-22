using WARD.Builder;
using WARD.Expressions;
using WARD.Runtime;
using WARD.Statements;
using WARD.Types;

// Setup builders.
ProgramBuilder pb = new ProgramBuilder();
UnitBuilder ub = pb.NewCompilationUnit("HelloWorld");
CodeBuilder cb = new CodeBuilder();

// Build the main function by first defining it then formally adding it as a function.
cb.Code(new StatementReturn(new ExpressionConstInt(VarType.Int, 0)));
ub.AddFunction(new Function(
    "main",
    new VarTypeFunction(VarType.Int)
), cb.EndBuilding());

// Finish the unit and compile.
pb.EndCompilationUnit(ub);
CompiledProgram program = pb.Compile();

// Run the program.
program.Execute();