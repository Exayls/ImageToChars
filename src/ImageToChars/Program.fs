module main

open System
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.Fonts;
open Helper
open ImageToChars.Image

// let width = Console.WindowWidth
// let height = Console.WindowHeight
let width = 30
let height = 8
printfn "%d %d" width height
let accuracy = 21
let imagePath = "ressources/image4.jpg"


// DisplayImage width height accuracy imagePath
let image = timeit (fun _ -> DisplayImage width height accuracy imagePath) 
Display image

