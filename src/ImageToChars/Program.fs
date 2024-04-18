namespace ImageToChars
module ImageToChars =

    open System
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
            not (Seq.exists (fun (i, j) -> (abs (arr1.[i, j] - arr2.[i, j])) > acc) indices)

    let Transpose (array2d:'a[,])= 
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

    // let Resize (array2d:'float[,]) width height= 
    //     Array2D.init width height (fun x y -> Mean (GetFilter x y array2d width height))

    let Mean (array:float[,]) = 
        let width = Array2D.length1 array
        let height = Array2D.length2 array
        let seq =seq{
            for row in 0 .. width - 1 do
                    for col in 0 .. height - 1 -> array[row,col]
        }
        (Seq.sum seq)/float(height*width)

    let GetFilter (x:int) (y:int) (array:float[,]) (width:int) (height:int) =
        let ratioY = (float(Array2D.length2 array)/float(height))
        let ratioX = (float(Array2D.length1 array)/float(width))

        let firstFloatY = (ratioY*float(y))
        let firstFloatX = (ratioX*float(x))
        let lastFloatY = (ratioY*float(y+1))
        let lastFloatX = (ratioX*float(x+1))

        let firstY = int(floor(firstFloatY))
        let firstX = int(floor(firstFloatX))
        let lastY = int(ceil(lastFloatY))
        let lastX = int(ceil(lastFloatX))

        let newWidth = lastX - firstX
        let newHeight = lastY - firstY

        let firstYWeight = 1.0-abs(float(firstY)-firstFloatY)
        let lastYWeight = 1.0-abs(float(lastY)-lastFloatY)
        (
        Array2D.init (int newWidth) (int newHeight) (fun x1 y1 -> array[firstX+x1, firstY+y1]) ,
        Array2D.init (int newWidth) (int newHeight) (fun x1 y1 ->
            if y1 = 0
                then firstYWeight
            elif y1 = newHeight-1
                then lastYWeight
            else float(1))
        )
 





