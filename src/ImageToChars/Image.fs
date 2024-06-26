namespace ImageToChars
module Image =

    open System
    open SixLabors.ImageSharp
    open SixLabors.ImageSharp.Processing
    open SixLabors.ImageSharp.PixelFormats
    open SixLabors.ImageSharp.Drawing.Processing
    open SixLabors.Fonts;
    open Helper


    let timeit f = 
        let watch = new System.Diagnostics.Stopwatch()
        watch.Start()
        let res = f() 
        watch.Stop()
        printfn "Needed %f ms" (watch.Elapsed.TotalMilliseconds)
        res

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


    let Mean (array:float[,]) = 
        let width = Array2D.length1 array
        let height = Array2D.length2 array
        let seq =seq{
            for row in 0 .. width - 1 do
                    for col in 0 .. height - 1 -> array[row,col]
        }
        (Seq.sum seq)/float(height*width)

    let Sum (array:float[,]) = 
        let width = Array2D.length1 array
        let height = Array2D.length2 array
        let seq =seq{
            for row in 0 .. width - 1 do
                    for col in 0 .. height - 1 -> array[row,col]
        }
        (Seq.sum seq)

    let WeightedMean ((array:float[,]), (weight:float[,])) = 
        let width = Array2D.length1 array
        let height = Array2D.length2 array
        let seqArray =seq{
            for row in 0 .. width - 1 do
                    for col in 0 .. height - 1 -> array[row,col]*weight[row,col]
        }
        let seqWeight =seq{
            for row in 0 .. width - 1 do
                    for col in 0 .. height - 1 -> weight[row,col]
        }
        (Seq.sum seqArray)/(Seq.sum seqWeight)


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
        let firstXWeight = 1.0-abs(float(firstX)-firstFloatX)
        let lastXWeight = 1.0-abs(float(lastX)-lastFloatX)
        (
        Array2D.init (int newWidth) (int newHeight) (fun x1 y1 -> array[firstX+x1, firstY+y1]) ,
        Array2D.init (int newWidth) (int newHeight) (fun x1 y1 ->
            let YWeight =
                if y1 = 0
                    then firstYWeight
                elif y1 = newHeight-1
                    then lastYWeight
                else 1.0
            let XWeight =
                if x1 = 0
                    then firstXWeight
                elif x1 = newWidth-1
                    then lastXWeight
                else 1.0
            YWeight * XWeight
            )
        )
 

    let Resize (array2d:float[,]) width height=
        Array2D.init width height (fun x y -> WeightedMean (GetFilter x y array2d width height) )


    let GetImageFromFont (fontPath:string) sizeFont sizeImage  charToDraw=
        let collection = new FontCollection()
        let family = collection.Add(fontPath)
        let font = family.CreateFont(float32(int(sizeFont)), FontStyle.Regular);

        let (x, y) = sizeImage
        let image = new Image<Rgba32>(x, y);
        let position = (PointF(float32(0),float32(4)))
        let textOptions = new RichTextOptions(font)
        textOptions.Origin <- position
        textOptions.HorizontalAlignment <- HorizontalAlignment.Left
        textOptions.VerticalAlignment <- VerticalAlignment.Top

        image.Mutate  ( fun x ->
                x.Fill(Color.White)|> ignore
                x.DrawText(textOptions, string(charToDraw), Color.Black)|> ignore
            )
        image


    let SquaredError (array:float[,]) (array2:float[,])=
        Sum (map2 (fun a1 a2 -> pown (a1-a2) 2) array array2)

    let Display (charArray:char[,]) = 
        let width = Array2D.length1 charArray
        let height = Array2D.length2 charArray
        for col in 0 .. height - 1 do 
            printf "\n"
            for row in 0 .. width - 1 do
                printf "%c" charArray[row,col]


    let DisplayImage (width:int) (height:int) (accuracy:int) (imagePath:string) =
        let image = Image.Load<Rgba32>(imagePath)
        let imageArray = Array2D.map (fun a  -> 1.0-a) (Resize (GetArrayFrom(image)) (width*accuracy) (height*accuracy*2))

        let bestArray = Array2D.create width height ((' ', 0.0))
        let ReadLines filePath = System.IO.File.ReadLines(filePath)
        printfn "1"
        let seq = seq {
            for i in ( ReadLines "ressources/chars" ) do
                let currentChar = char(i)
                let image = GetImageFromFont "ressources/myFont.ttf" 34 (21,42) (currentChar)
                let charArray = Resize (GetArrayFrom image) accuracy (accuracy*2)
                (currentChar,charArray)
            }
        printfn "2"
        let charArrayFinal = Array2D.map (fun (c,_)-> c) (Seq.fold (fun (acc:(char*float)[,]) ((currentChar, charArray):char*float[,]) ->
            let GetBestChar = (fun xCharArray yCharArray ->
                let partialImageArray = Array2D.init (accuracy) (accuracy*2) (fun xPartial yPartial ->
                    imageArray[xCharArray*(accuracy)+xPartial, yCharArray*(accuracy*2)+yPartial]
                )
                let m = SquaredError partialImageArray charArray
                let _, curentError = acc[xCharArray,yCharArray]
                let validity = 1.0/(m+1.0)
                if validity > curentError
                    then
                    (currentChar, validity)
                else
                    acc[xCharArray,yCharArray]
            )
            printfn "%c %d %d" currentChar width height
            timeit (fun _ -> Array2D.init width height GetBestChar)
        ) bestArray seq)

        charArrayFinal
