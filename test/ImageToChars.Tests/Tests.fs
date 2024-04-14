module Tests


open System
open Xunit
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.PixelFormats
open Helper

let GetArrayFrom (image: Image<Rgba32>) : float[,]= 
    let pixels :Rgba32[,]= Helper.ImageSharp.GetPixelArray image
    let ComputeIntensity (a:Rgba32) = float (float(int a.R + int a.G + int a.B)/float(3*255))
    let intensityArray = Array2D.map ComputeIntensity pixels
    intensityArray

let AreEquals (arr1: 'a [,]) (arr2: 'a [,]) : bool =
    if arr1.GetLength(0) <> arr2.GetLength(0) || arr1.GetLength(1) <> arr2.GetLength(1) then
        false
    else
        let rows = arr1.GetLength(0)
        let cols = arr1.GetLength(1)
        let indices = seq { for i in 0 .. rows - 1 do for j in 0 .. cols - 1 -> (i, j) }
        not (Seq.exists (fun (i, j) -> arr1.[i, j] <> arr2.[i, j]) indices)

let AreEqualsWithAcc (arr1: float [,]) (arr2: float [,]) (acc: float) : bool =
    if arr1.GetLength(0) <> arr2.GetLength(0) || arr1.GetLength(1) <> arr2.GetLength(1) then
        false
    else
        let rows = arr1.GetLength(0)
        let cols = arr1.GetLength(1)
        let indices = seq { for i in 0 .. rows - 1 do for j in 0 .. cols - 1 -> (i, j) }
        not (Seq.exists (fun (i, j) -> arr1.[i, j] - arr2.[i, j] > acc) indices)

let Transpose array2d= 
    Array2D.init (array2d |> Array2D.length2) (array2d |> Array2D.length1) (fun r c -> array2d.[c,r])

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
    let arr1: float[,] =
        array2D
            [ [ 0; 0; 0 ]
              [ 1; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let arr2: float[,] =
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
    let arr1: float[,] =
        array2D
            [ [ 0; 0; 0 ]
              [ 1; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let arr2: float[,] =
        array2D
            [ [ 0; 0; 0 ]
              [ 1; 1; 0 ]
              [ 0; 1; 0 ]
              [ 0; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let equality = AreEquals arr2 arr1
    Assert.False(equality)

[<Fact>]
let ``when_calling_GetArray_With_T_should_get_image_coresponding`` () =
    let TFilter: float[,] =
        Transpose (
        array2D
            [ [ 1; 1; 1 ]
              [ 0; 0; 0 ]
              [ 1; 0; 1 ]
              [ 1; 0; 1 ]
              [ 1; 0; 1 ]
              [ 1; 1; 1 ] ]
        )
    let image = Image.Load<Rgba32>("ressources/T.png")
    let imageArray = GetArrayFrom(image)
    Assert.True(AreEqualsWithAcc imageArray TFilter 0.1)

[<Fact>]
let ``when_calling_GetArray_With_I_should_get_image_coresponding`` () =
    let TFilter: float[,] =
        Transpose (
        array2D
            [ [ 1; 1; 1 ]
              [ 1; 0.5; 1 ]
              [ 1; 0.5; 1 ]
              [ 1; 0.5; 1 ]
              [ 1; 0.5; 1 ]
              [ 1; 1; 1 ] ]
        )
    let image = Image.Load<Rgba32>("ressources/I.png")
    let imageArray = GetArrayFrom(image)
    printf "%A" (Transpose imageArray)
    Assert.True(AreEqualsWithAcc imageArray TFilter 0.1)


[<Fact>]
let ``when_calling_GetArray_With_E_should_get_image_coresponding`` () =
    let TFilter: float[,] =
        Transpose (
        array2D
            [ [ 1; 1; 1 ]
              [ 0.78; 0.78; 0.78 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 1 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 0.78 ] ]
        )
    let image = Image.Load<Rgba32>("ressources/E.png")
    let imageArray = GetArrayFrom(image)
    printf "%A" (Transpose imageArray)
    Assert.True(AreEqualsWithAcc imageArray TFilter 0.1)


