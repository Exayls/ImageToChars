module Tests

open System
open Xunit

let GetArrayFrom (image: Image) : int[,]= 
    array2D [[]]

let AreEquals (arr1: 'a [,]) (arr2: 'a [,]) : bool =
    if arr1.GetLength(0) <> arr2.GetLength(0) || arr1.GetLength(1) <> arr2.GetLength(1) then
        false
    else
        let rows = arr1.GetLength(0)
        let cols = arr1.GetLength(1)
        let indices = seq { for i in 0 .. rows - 1 do for j in 0 .. cols - 1 -> (i, j) }
        not (Seq.exists (fun (i, j) -> arr1.[i, j] <> arr2.[i, j]) indices)

let map2 (func:'a -> 'a -> 'a) (array1:'a[,]) (array2:'a[,]) = 
    if array1.GetLength(0) <> array2.GetLength(0) || array1.GetLength(1) <> array2.GetLength(1) then
                failwith "Arrays must have the same dimensions."
    let rows = array1.GetLength(0)
    let cols = array2.GetLength(1)
    let result = Array2D.create rows cols (func array1.[0, 0] array2.[0, 0])
    for i in 0 .. rows - 1 do
        for j in 0 .. cols - 1 do
            result.[i, j] <- func array1.[i, j] array2.[i, j]
    result

[<Fact>]
let ``when_arrays_are_equals_AreEquals_should_returns_true`` () =
    let arr1: int[,] =
        array2D
            [ [ 0; 0; 0 ]
              [ 1; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let arr2: int[,] =
        array2D
            [ [ 0; 0; 0 ]
              [ 1; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let equality = AreEquals arr2 arr1
    Assert.True(equality)


[<Fact>]
let ``when_arrays_are_equals_AreEquals_should_returns_false`` () =
    let arr1: int[,] =
        array2D
            [ [ 0; 0; 0 ]
              [ 1; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let arr2: int[,] =
        array2D
            [ [ 0; 0; 0 ]
              [ 1; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let equality = AreEquals arr2 arr1
    Assert.False(equality)

