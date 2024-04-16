module Tests


open System
open Xunit
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.PixelFormats
open Helper
open ImageToChars.ImageToChars


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
    printf "%A" (Transpose imageArray)
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
    printf "%A" (Transpose imageArray)
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

//     let reduced: float[,] =
//         array2D
//             [ [ 1; 1; 1 ]
//               [ 0.78; 0.78; 0.78 ]
//               [ 0.78; 1; 1 ]
//               [ 0.78; 0.78; 1 ]
//               [ 0.78; 1; 1 ]
//               [ 0.78; 0.78; 0.78 ] ]
//     Assert.True()


