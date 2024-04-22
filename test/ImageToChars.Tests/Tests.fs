module Tests


open System
open Xunit
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.Fonts;
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
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.001)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.001)


[<Fact>]
let ``Filter_should_return_half_weight_if_half_case_interpolation`` () =
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
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.001)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.001)

[<Fact>]
let ``Filter_should_return_half_weight_if_half_case_interpolation_2`` () =
    let array: float[,] =
        array2D
            [ [ 1.00; 0.10; 0.30; 0.7; 0.9 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [ 1; 0.10] ]
    let expectedWeight: float[,] =
        array2D
            [ [1; 0.666] ]
    let (filter, weight) = GetFilter 0 0 array 1 3
    // printfn "%A" expectedWeight
    // printfn "%A" weight
    // printfn "%A" expectedFilter
    // printfn "%A" filter
    // printfn "aaaa"
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.01)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.01)


[<Fact>]
let ``Filter_should_return_half_weight_if_half_case_interpolation_3`` () =
    let array: float[,] =
        array2D
            [ [ 1.00; 0.10; 0.30; 0.7; 0.9 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [ 0.10; 0.30; 0.70 ] ]
    let expectedWeight: float[,] =
        array2D
            [ [0.3333; 1; 0.3333] ]
    let (filter, weight) = GetFilter 0 1 array 1 3
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.01)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.01)

[<Fact>]
let ``Filter_should_return_half_weight_if_half_case_interpolation_4`` () =
    let array: float[,] =
        array2D
            [ [ 1.00; 0.10; 0.30 ] ]
    let expectedFilter: float[,] =
        array2D
            [ [1; 0.10 ] ]
    let expectedWeight: float[,] =
        array2D
            [ [0.4; 0.2] ]
    let (filter, weight) = GetFilter 0 1 array 1 5
    // printfn "%A" expectedWeight
    // printfn "%A" weight
    // printfn "%A" expectedFilter
    // printfn "%A" filter
    Assert.True(AreEqualsWithAcc expectedFilter filter 0.01)
    Assert.True(AreEqualsWithAcc expectedWeight weight 0.01)


[<Fact>]
let ``weighted_mean`` () =
    let array: float[,] =
        array2D
            [ [ 4; 3 ]
              [ 2; 1 ] ]
    let weight: float[,] =
        array2D
            [ [ 1000; 100 ]
              [ 10; 1 ] ]

    Assert.True(FloatEquals 3.889 (WeightedMean (array,weight)) 0.01)


[<Fact>]
let ``when_resize_array_by_half_should_be_reduced`` () =
    let array: float[,] =
        array2D
            [ [ 1; 1; 1 ]
              [ 0.78; 0.78; 0.78 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 1 ]
              [ 0.78; 1; 1 ]
              [ 0.78; 0.78; 0.78 ] ]

    let image = Image.Load<Rgba32>("ressources/E_adjusted.png")
    let expectedReduced = Transpose (GetArrayFrom(image))
    let reduced = (Resize array 3 3)

    Assert.True(AreEqualsWithAcc expectedReduced reduced 0.01)

[<Fact>]
let ``GetImageFromFont_should_return_char`` () =

    let charNumber = "f8a5"

    let fontPath = "ressources/myFont.ttf"
    let image = GetImageFromFont fontPath 34 (21,32) (char(Convert.ToInt32((charNumber), 16)))
    image.Save("aie.png")
    let array = GetArrayFrom image
    let reducedArray = Resize array 4 8

    let expetedArray: float[,] =
        array2D
            [ 
                [ 1.0; 1.0; 0.70; 0.43; 0.55; 0.55; 0.55; 0.35 ]
                [ 1.0; 1.0; 0.65; 0.69; 0.79; 0.94; 0.96; 0.43 ]
                [ 1.0; 1.0; 0.65; 0.69; 0.58; 0.60; 0.71; 0.43 ]
                [ 1.0; 1.0; 0.70; 0.43; 0.55; 0.55; 0.55; 0.36 ]
            ]

    Assert.True(AreEqualsWithAcc expetedArray reducedArray 0.01)


[<Fact>]
let ``explore`` () =

    let ReadLines filePath = System.IO.File.ReadLines(filePath)
    printfn "1"
    let seq = seq {
        for i in ( ReadLines "ressources/chars" ) do
            let number = (char(Convert.ToInt32((i), 16)))
            let image = GetImageFromFont "ressources/myFont.ttf" 34 (21,42) (char(number))
            let imageArray = Resize (GetArrayFrom image) 12 24
            imageArray
        }
    printfn "2"
    Seq.iter (fun array -> printfn "%A" (Array2D.length2 array)) seq
    printfn "3"
        

    // let badImage = GetImageFromFont "ressources/myFont.ttf" 34 (21,42) "􏿿"
    // let badArray = Resize (GetArrayFrom badImage) 8 16

    // let seq2 = seq {0x3fe10 .. 0x10ffff}
    // for i in ( seq2) do
    //     let image = GetImageFromFont "ressources/myFont.ttf" 34 (21,42) (Char.ConvertFromUtf32 i)
    //     let imageArray = Resize (GetArrayFrom image) 8 16
    //     // printfn "%x" i
    //     if not (AreEqualsWithAcc imageArray badArray 0.01)
    //         then printfn "%x" i
    Assert.True(true)
