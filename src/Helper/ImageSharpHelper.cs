using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Helper
{
    public class ImageSharp
    {
        static public Rgba32[,] GetPixelArray(Image<Rgba32> image)
        {
            var height = image.Height;
            var width = image.Width;
            var array = new Rgba32[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Rgba32 rgba32 = image[x, y];
                    array[x, y] = rgba32;
                }
            }
            return array;
        }
    }
}
