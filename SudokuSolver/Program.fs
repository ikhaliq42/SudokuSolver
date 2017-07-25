// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

module Sudoku

open Solver

[<EntryPoint>]
let main argv = 
    printfn "%A" (solvePuzzle argv.[0])
    0 // return an integer exit code
