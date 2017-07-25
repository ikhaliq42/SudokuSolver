module Solver

let gridSize = 9
let subGridsize = 3

// function to get row and column of flattened puzzle grid
let rowAndCol idx = 
    let r = idx / gridSize + 1
    let c = (idx+1) - (r-1) * gridSize
    r,c

// convert a string of digits representing the puzzle to a list of ints
let intStringToList intString = 
    intString |> Seq.map (string >> int) |> Seq.toList;; 

// convert a list of ints to a string
let intListToString intList = intList |> List.map (fun i -> i.ToString()) |> String.concat ""

// create initial solved set from flattened puzzle set
let intialSolvedSet (puzzleString: string) =
    let flatPuzzle = intStringToList puzzleString
    [ for i,k in (flatPuzzle |> List.indexed) do if k <> 0 then yield (rowAndCol i), k ]
    |> Map.ofList

// create initial unsolved set from flattened puzzle set
let intialUnsolvedSet (puzzleString: string) =   
    let flatPuzzle = intStringToList puzzleString 
    [ for i,k in (flatPuzzle |> List.indexed) do if k = 0 then yield rowAndCol i]
    |> Set.ofList

// check if cell i,j is in the same sub-grid as cell r,c
let sameSubGrid (r, c) (i,j) =
    let s = subGridsize
    let m = (r-1) / s * s + 1
    let n = (c-1) / s * s + 1 
    [ for i in m .. (m+s-1) do for j in n .. (n+s-1) -> (i,j) ]
    |> List.contains (i,j)

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

// calculate the solution and return as a list
let solvePuzzle puzzle = 

    let rec solver (solvedSet, unsolvedSet) =
        if unsolvedSet |> Set.isEmpty then 
            (solvedSet, unsolvedSet)
        else
            solver (sweep (solvedSet, unsolvedSet))
    
    let solvedSet, unsolvedSet = solver (intialSolvedSet puzzle, intialUnsolvedSet puzzle)
    let flatSoln = [ for r in 1 .. gridSize do 
                        for c in 1 .. gridSize do yield solvedSet |> Map.find (r,c) ]
    intListToString flatSoln