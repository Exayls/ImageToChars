module Tests


open System
open Xunit
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.PixelFormats
open Helper
open ImageToChars.ImageToChars

let FloatEquals x y precision=
    (x - precision) < y && y < (x + precision)

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
let ``when_arrays_are_not_equals_AreEquals_should_returns_false`` () =
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
let ``when_arrays_are_not_exactly_equals_AreEquals_should_returns_false`` () =
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
              [ 0; 1; 0.11 ]
              [ 0; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let equality = AreEqualsWithAcc arr2 arr1 0.1
    Assert.False(equality)

[<Fact>]
let ``when_arrays_are_almost_exactly_equals_AreEquals_should_returns_true`` () =
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
              [ 0; 1; 0.9 ]
              [ 0; 1; 1 ]
              [ 0; 1; 0 ]
              [ 0; 0; 0 ] ]

    let equality = AreEqualsWithAcc arr2 arr1 0.1
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
    Assert.True(AreEqualsWithAcc imageArray TFilter 0.01)

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
    Assert.True(AreEqualsWithAcc imageArray TFilter 0.01)


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
    Assert.True(AreEqualsWithAcc imageArray TFilter 0.01)

// [<Fact>]
// let ``when_resize_array_by_half_should_be_reduced`` () =
//     let array: float[,] =
//         array2D
//             [ [ 1; 1; 1 ]
//               [ 0.78; 0.78; 0.78 ]
//               [ 0.78; 1; 1 ]
//               [ 0.78; 0.78; 1 ]
//               [ 0.78; 1; 1 ]
//               [ 0.78; 0.78; 0.78 ] ]

//     let image = Image.Load<Rgba32>("ressources/E_adjusted.png")
//     let reduced = GetArrayFrom(image)
//     printf "%A" (Transpose reduced)
//     Assert.True(AreEquals reduced (Resize array 3 3))


[<Fact>]
let ``mean_should_return_mean_of_array_1`` () =
    let array: float[,] =
        array2D
            [ [ 1; 1 ]
              [ 1; 0 ] ]

    Assert.True(0.75 = (Mean array))

[<Fact>]
let ``mean_should_return_mean_of_array_2`` () =
    let array: float[,] =
        array2D
            [ [ 1; 1; 1 ]
              [ 0.78; 0.78; 0.78 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 1 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 0.78 ] ]
    Assert.True(FloatEquals (Mean array) 0.87777 0.00001)

[<Fact>]
let ``Filter_should_return_same_if_same_size`` () =
    let array: float[,] =
        array2D
            [ [ 1; 1; 1 ]
              [ 0.78; 0.78; 0.78 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 1 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 0.78 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [ 0.78;] ]
    let expectedWeight: float[,] =
        array2D
            [ [1] ]
    let (filter, weight) = GetFilter 1 1 array 6 3
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.001)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.001)


[<Fact>]
let ``Filter_should_return_same_if_same_size_2`` () =
    let array: float[,] =
        array2D
            [ [ 1; 1; 1 ]
              [ 0.78; 0.78; 0.78 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 1 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 0.78 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [ 1;] ]
    let expectedWeight: float[,] =
        array2D
            [ [1] ]
    let (filter, weight) = GetFilter 4 2 array 6 3
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.001)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.001)

[<Fact>]
let ``Filter_should_return_2_case_if_half_width`` () =
    let array: float[,] =
        array2D
            [ [ 1; 1; 1 ]
              [ 0.78; 0.78; 0.78 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 1 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 0.78 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [ 1;]
              [ 0.78;] ]
    let expectedWeight: float[,] =
        array2D
            [ [1]; [1] ]
    let (filter, weight) = GetFilter 0 0 array 3 3
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.001)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.001)

[<Fact>]
let ``Filter_should_return_2_case_if_half_size`` () =
    let array: float[,] =
        array2D
            [ [ 1; 1; 1; 1 ]
              [ 0.78; 0.78; 0.78; 1 ]
              [ 0.78; 1; 1; 0.5 ]
              [ 0.78; 0.78; 1; 1 ]
              [ 0.78; 1; 1; 0.5 ]
              [ 0.78; 0.78; 0.78; 1 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [ 1; 0.5;] ]
    let expectedWeight: float[,] =
        array2D
            [ [1; 1] ]
    let (filter, weight) = GetFilter 2 1 array 6 2
    printfn "%A" expectedFilter
    printfn "%A" filter
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.001)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.001)


[<Fact>]
let ``Filter_should_return_half_case_if_double_size`` () =
    let array: float[,] =
        array2D
            [ [ 1.00; 0.10; 0.30 ]
              [ 0.80; 0.50; 0.60 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [ 1; 0.10;] ]
    let expectedWeight: float[,] =
        array2D
            [ [1; 0.5] ]
    let (filter, weight) = GetFilter 0 0 array 2 2
    // printfn "%A" expectedWeight
    // printfn "%A" weight
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.001)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.001)




