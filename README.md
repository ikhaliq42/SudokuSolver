# SudokuSolver
An F# program for solving sudoku puzzles

A simple command line based F# program for solving Sudoku puzzles. 

Compiling:

This is a Visual Studio 2015 project - to compile, open the solution file SudokuSolver.sln in Visual Studio, and select Build > Build Solution

Command line usage:

    SudokuSolver <puzzle>

..where <puzzle> the unsolved sudoku puzzle flattened into single line, with empty cells represented by zeros.

For example, if we have a puzzle that looks as such:

    0 0 4 3 0 0 2 0 9
    0 0 5 0 0 9 0 0 1
    0 7 0 0 6 0 0 4 3
    0 0 6 0 0 2 0 8 7
    1 9 0 0 0 7 4 0 0
    0 5 0 0 8 3 0 0 0
    6 0 0 0 0 0 1 0 5
    0 0 3 5 0 8 6 9 0
    0 4 2 9 1 0 3 0 0

Then the command line will be as follows:

    SudokuSolver 004300209005009001070060043006002087190007400050083000600000105003508690042910300

The output will the solution in the same flat format.

TODO:

    - Validation on inputs - there is some validation, but it is not comprehensive
    - Exception handling
