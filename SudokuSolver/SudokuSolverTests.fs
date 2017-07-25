module SudokuSolverTests

open Solver
open NUnit.Framework
open FSharp.Data

type Puzzle = CsvProvider<"sudoku.csv">

[<Test>]
let ``Test rowAndCol`` () =
    Assert.AreEqual((1,1), rowAndCol 0)
    Assert.AreEqual((1,6), rowAndCol 5)
    Assert.AreEqual((3,6), rowAndCol 23)
    Assert.AreEqual((9,9), rowAndCol 80)

[<Test>]
let ``Test intStringToList`` () =
    let func = intStringToList
    let input = "61971"
    let expectedOutput = [6;1;9;7;1]
    let actualOutput = func input 
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test intListToString`` () =
    let func = intListToString
    let input = [6;1;9;7;1]
    let expectedOutput = "61971"
    let actualOutput = func input 
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test intialSolvedSet`` () =
    let func = intialSolvedSet
    let input = "004300000000000000000000000000000000000000000000000000000000000000090000000000005"
    let expectedOutput = [((1,3),4);((1,4),3);((8,5),9);((9,9),5)] |> Map.ofList
    let actualOutput = func input 
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test intialUnsolvedSet`` () =
    let func = intialUnsolvedSet
    let input = "120056789123456789123456789123456789123456789123456789123456789123406789123456780"
    let expectedOutput = [(1,3);(1,4);(8,5);(9,9)] |> Set.ofList
    let actualOutput = func input 
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test sameSubGrid`` () =
    let func = sameSubGrid
    let input = [(1,1);(4,4);(7,8);(9,9);(6,6)] 
                |> List.zip [(3,3);(2,7);(7,8);(9,7);(3,7)]
    let expectedOutput = [true;false;true;true;false]
    let actualOutput = [for i1,i2 in input do yield func i1 i2 ]
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test cannotBe`` () =
    let func = cannotBe
    let ((r,c),s) = ((5,5),[((5,1),1);((2,5),2);((4,4),3);((2,2),4)] |> Map.ofList)
    let expectedOutput = [1;2;3]    
    let actualOutput = func r c s |> List.sort
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test cellSoln when no solution`` () =
    let func = cellSoln
    let ((r,c),s) = ((5,5),[((5,1),1);((2,5),2);((4,4),3);((2,2),4)] |> Map.ofList)
    let expectedOutput = 0    
    let actualOutput = cellSoln r c s
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test cellSoln when is a solution`` () =
    let func = cellSoln
    let cells = [(5,1);(5,2);(5,7);(5,9);(1,5);(3,5);(4,5);(8,5);(2,2)]
    let vals = [1;2;3;4;5;6;7;8;9]
    let ((r,c),s) = ((5,5), vals |> List.zip cells |> Map.ofList)
    let expectedOutput = 9    
    let actualOutput = cellSoln r c s
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test sweep when no solutions`` () =
    let func = sweep
    let cells = [(5,1);(5,2);(5,7);(5,9);(1,5);(3,5);(4,5);(8,5);(2,2)]
    let vals = [1;2;3;4;4;6;7;8;9]
    let solved = vals |> List.zip cells |> Map.ofList
    let unsolved = [(1,1);(5,5)] |> Set.ofList
    let expectedOutput = (solved, unsolved)   
    let actualOutput = func (solved, unsolved)
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test sweep when is solution`` () =
    let func = sweep
    let cells = [(5,1);(5,2);(5,7);(5,9);(1,5);(3,5);(4,5);(8,5);(2,2)]
    let vals = [1;2;3;4;5;6;7;8;9]
    let solved = vals |> List.zip cells |> Map.ofList
    let unsolved = [(1,1);(5,5)] |> Set.ofList
    let expectedOutput = (solved |> Map.add (5,5) 9, unsolved |> Set.remove (5,5))   
    let actualOutput = func (solved, unsolved)
    Assert.AreEqual(expectedOutput, actualOutput)

[<Test>]
let ``Test whole puzzle solution`` () =
    let puzzleString = "004300209005009001070060043006002087190007400050083000600000105003508690042910300"
    let solutionString = "864371259325849761971265843436192587198657432257483916689734125713528694542916378"
    let mySolutionString = solvePuzzle puzzleString
    Assert.AreEqual(solutionString, mySolutionString)

[<Test>]
let ``Test a batch of puzzles`` () =
    let puzzles = Puzzle.Load("..\..\sudoku.csv").Cache()
    let puzzleStrings = [ for row in puzzles.Rows do yield row.Puzzle ]
    let solutionStrings = [ for row in puzzles.Rows do yield row.Solution ]
    let mySolutionStrings = [for puzStr in puzzleStrings do yield solvePuzzle puzStr ]
    Assert.AreEqual(solutionStrings,mySolutionStrings)
