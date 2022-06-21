using WARD.Builder;
using WARD.Expressions;
using WARD.Statements;
using WARD.Types;

// Setup builders.
ProgramBuilder pb = new ProgramBuilder();
UnitBuilder ub = pb.NewCompilationUnit("Dummy");
CodeBuilder cb = new CodeBuilder();

// Build the main function.
cb.Code(new StatementReturn(new ExpressionConstInt(VarType.Int, 0)));
ub.AddFunction(new Function(
    "main",
    new VarTypeFunction(VarType.Int)
), cb.EndBuilding());

// Finish the unit and compile.
pb.EndCompilationUnit(ub);
pb.Compile();

// Run code.
pb.Execute();