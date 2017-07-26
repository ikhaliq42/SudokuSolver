module SudokuSolverTests

open Solver
open NUnit.Framework
open FSharp.Data

type Puzzle = CsvProvider<"sudoku.csv">

[<Test>]
let ``Test a valid 9x9 puzzle`` () =
    let puzzleString = "004300209005009001070060043006002087190007400050083000600000105003508690042910300"
    let solutionString = "864371259325849761971265843436192587198657432257483916689734125713528694542916378"
    let mySolutionString = solvePuzzle puzzleString
    Assert.AreEqual(solutionString, mySolutionString)

[<Test>]
let ``Test an invalid puzzle`` () =
    let puzzleString = "0043002090050090010700600430060020871900074000500830006000001050035086900"
    let solutionString = "Error: Invalid puzzle."
    let mySolutionString = solvePuzzle puzzleString
    Assert.AreEqual(solutionString, mySolutionString)

[<Test>]
let ``Test a batch of puzzles`` () =
    let puzzles = Puzzle.Load("..\..\sudoku.csv").Cache()
    let puzzleStrings = [ for row in puzzles.Rows do yield row.Puzzle ]
    let solutionStrings = [ for row in puzzles.Rows do yield row.Solution ]
    let mySolutionStrings = [for puzStr in puzzleStrings do yield solvePuzzle puzStr ]
    Assert.AreEqual(solutionStrings,mySolutionStrings)
