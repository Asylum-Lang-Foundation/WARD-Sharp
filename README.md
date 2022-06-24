# WARD-Sharp
Ward API Redefines Development (WARD) - An open source framework for developing general or domain specific computer languages.

## Note
Unfortunately, WARD is in a BETA state and is thus not completely finished as its features are being developed. That shouldn't stop you from trying it out though!

## What Is WARD?
Have you ever wanted to make your programming language but learning how to do proper codegen is insanity? Spend some time with WARD, and you'll be breezing through it! WARD takes a high level, generic approach at code generation to make it less like low level assembly, and more like a generic programming language. Powered by LLVM, this allows you to output compiled object code of your language or interpret it on the fly!

## Features
* High level method of code generation.
* Rich type system that supports everything from custom enums to structs to aliases along with built-in type checking.
* Expression system to handle any operation you could need.
* Automatic scope and variable resolution that handles features such as function overloads for you.
* Built-in operators and easy way to declare custom ones.
* Powerful generics system that utilizes concepts to fine tune which types can be used in them.
* Support for simultaneous compilation unit and function building for allowing max parallelization and performance.
* Export LLVM assembly, LLVM bitcode, assembly, object code, or run your code directly in a JIT!
* And more to come!

## Sample
Below is an example of a hello world program written with WARD. You can find more examples in the `Tests` folder.

```cs
using WARD.Builder;
using WARD.Common;
using WARD.Expressions;
using WARD.Runtime;
using WARD.Statements;
using WARD.Types;

// Setup builders.
// A program builder represents the entire program, while a unit builder allows us to build a unit or "file".
// You can build with multiple unit builders at the same time in parallel!
ProgramBuilder pb = new ProgramBuilder();
UnitBuilder ub = pb.NewCompilationUnit("HelloWorld");

// External function for printing statements.
// This is part of the C library, so we must tell the compiler to not mangle the name.
// Notice that it has no definition.
// The input parameter is a constant, so we only give it the read permission.
ub.AddFunction(new Function(
    "puts",
    new VarTypeFunction(VarType.Int, null, new Variable("str", VarType.String, DataAccessFlags.Read)),
    new ItemAttribute("NoMangle")
));

// Build the main function by first defining it, then formally adding it as a function.
// The main function is special and will not be mangled.
// Notice that we don't have to add the code to the function immediately like we do here, we could have prepared it in advance.
// A function is a variable, so we can just fetch it with a variable expression and WARD will automatically resolve it.
CodeBuilder cb = new CodeBuilder();
cb.Code(new ExpressionCall(new ExpressionVariable("puts"), new ExpressionConstString("Hello World!")));
cb.Code(new StatementReturn(new ExpressionConstInt(VarType.Int, 0)));
ub.AddFunction(new Function(
    "main",
    new VarTypeFunction(VarType.Int)
), cb.EndBuilding());

// Finish the unit and compile.
pb.EndCompilationUnit(ub);
CompiledProgram program = pb.Compile();

// Sample of how to export a compilation unit to various output formats.
program.ExportLLVMAssembly("HelloWorld", "HelloWorld.ll");
program.ExportLLVMBitcode("HelloWorld", "HelloWorld.bc");
program.ExportAssembly("HelloWorld", "HelloWorld.s");
program.ExportObject("HelloWorld", "HelloWorld.o");

// And/or you can run it directly!
program.Execute();
```