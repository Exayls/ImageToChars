module main

open System
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.Fonts;
open Helper
open ImageToChars.Image

let width = Console.WindowWidth
let height = Console.WindowHeight
let accuracy = 21
let imagePath = "ressources/image3.jpg"


DisplayImage width height accuracy imagePath
