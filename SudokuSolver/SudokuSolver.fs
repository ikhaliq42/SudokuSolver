module Solver

open System

exception UnsolvableException of string

// convert a string of digits representing the puzzle to a list of ints
let intStringToList intString = intString |> Seq.map (string >> int) |> Seq.toList 

// convert a list of ints to a string
let intListToString intList = intList |> List.map (fun i -> i.ToString()) |> String.concat ""

// check if puzzle is a valid size - must be a perfect square and perfect cube
let isValidSize puzzle =    
    let l = String.length puzzle
    let square_root = sqrt (float l)
    let cube_root = sqrt (square_root)
    let square_root_trunc =  truncate square_root
    let cube_root_trunc = truncate cube_root
    if square_root = square_root_trunc && cube_root = cube_root_trunc then true else false
    
// check if puzzle content is valid i.e. digits only
let isValidContent puzzle =
    puzzle |> Seq.skipWhile(Char.IsDigit) |> Seq.isEmpty

// check that puzzle is valid - various tests can be added here
let isValidPuzzle puzzle = isValidSize puzzle && isValidContent puzzle

// calculate the solution and return as a list
let solution puzzle = 

    // function to get row and column of flattened puzzle grid
    let rowAndCol gridSize idx = 
        let r = idx / gridSize + 1
        let c = (idx+1) - (r-1) * gridSize
        r,c
    
    // get grid size of puzzle
    let gridSize = int (sqrt (float (String.length puzzle)))

    // get sub grid size of puzzle
    let subGridsize = int (sqrt (float (gridSize)))

    // check if cell i,j is in the same sub-grid as cell r,c
    let sameSubGrid (r, c) (i,j) =
        let s = subGridsize
        let m = (r-1) / s * s + 1
        let n = (c-1) / s * s + 1 
        [ for i in m .. (m+s-1) do for j in n .. (n+s-1) -> (i,j) ]
        |> List.contains (i,j)

    // create initial solved set from flattened puzzle set
    let intialSolvedSet (puzzleString: string) =
        let flatPuzzle = intStringToList puzzleString
        [ for i,k in (flatPuzzle |> List.indexed) do if k <> 0 then yield (rowAndCol gridSize i), k ]
        |> Map.ofList

    // create initial unsolved set from flattened puzzle set
    let intialUnsolvedSet (puzzleString: string) =   
        let flatPuzzle = intStringToList puzzleString 
        [ for i,k in (flatPuzzle |> List.indexed) do if k = 0 then yield rowAndCol gridSize i]
        |> Set.ofList
    // find all solved values in the same row, column or sub-grid as cell r,c
    // but not r,c itself
    let cannotBe r c solutionSet =
        [ for (i,j),k in solutionSet |> Map.toList do 
            if (i = r || j = c || sameSubGrid (r,c) (i,j)) && (i,j) <> (r,c) then yield k ]

    // check if an unsolved cell has a solution, and return solution if found, 
    // or zero otherwise
    let cellSoln r c solutionSet =
        let possibleValues = [1 .. gridSize] |> List.except (cannotBe r c solutionSet)
        if possibleValues |> List.length = 1 then  
            possibleValues |> List.exactlyOne
        else  
            0

    // go through unsolved set and solve any cells that can be solved, 
    // and add newly solved cells to solved set
    let sweep (solvedSet, unsolvedSet) =
        let newSolvedSet = [ for r,c in unsolvedSet do yield (r,c),(cellSoln r c solvedSet) ]
                           |> List.filter(fun (i,k) -> k <> 0)
                           |> List.append (solvedSet |> Map.toList)
                           |> Map.ofList
        let newUnsolvedSet = [ for r,c in unsolvedSet do 
                                    if not (newSolvedSet |> Map.containsKey (r,c)) then yield r,c ] 
                             |> Set.ofList
        newSolvedSet, newUnsolvedSet

    let rec solver ((solvedSet, unsolvedSet), lastUnsolvedCount) =
        let currentUnsolvedSize = unsolvedSet |> Set.count
        if currentUnsolvedSize = lastUnsolvedCount then
            raise (UnsolvableException(sprintf "Puzzle is unsolvable"))
        elif unsolvedSet |> Set.isEmpty then 
            (solvedSet, unsolvedSet), currentUnsolvedSize
        else
            solver (sweep (solvedSet, unsolvedSet), currentUnsolvedSize)
    
    let initSolvedSet = intialSolvedSet puzzle
    let initUnsolvedSet = intialUnsolvedSet puzzle
    let ((solvedSet, unsolvedSet), l) = solver ((initSolvedSet, initUnsolvedSet), -1)

    [ for r in 1 .. gridSize do for c in 1 .. gridSize do yield solvedSet |> Map.find (r,c) ]

let solvePuzzle puzzle =
    
    if isValidPuzzle puzzle then 
        intListToString (solution puzzle)
    else 
        raise (ArgumentException("Invalid puzzle."))
