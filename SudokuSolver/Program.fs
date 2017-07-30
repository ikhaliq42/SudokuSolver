// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

module Sudoku

open Solver
open System

[<EntryPoint>]
let main argv = 
    if argv.Length = 0 then
        printfn "Usage: SudokuSolver <puzzle>"
        0
    else
        try
            printfn "%A" (solvePuzzle argv.[0])
            0 // return an integer exit code
        with
            | :? ArgumentException as e     -> printfn "Error: %s" e.Message; 1
            | UnsolvableException(msg)   -> printfn "Error: %s" msg; 2
