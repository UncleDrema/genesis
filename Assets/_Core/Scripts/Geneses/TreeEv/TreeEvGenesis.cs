using Genesis.Common.Components;
using Genesis.GameWorld;

namespace Geneses.TreeEv
{
    public class TreeEvGenesis : IGenesis
    {
        public IPixel CreatePixel(int width, int height, int x, int y)
        {
            return new TreeEvPixel()
            {
                Type = GetType(width, height, x, y),
                Gene = -1
            };
        }

        public void PostProcess(ref WorldComponent cWorld)
        {
            var pixels = cWorld.Pixels;
            var width = cWorld.Width;
            var height = cWorld.Height;
            cWorld.ForEach<TreeEvPixel>((i, j, pixel) =>
            {
                pixel.Above = (TreeEvPixel) pixels.Up(width, height, i, j);
                pixel.Under = (TreeEvPixel) pixels.Down(width, height, i, j);
                pixel.Left = (TreeEvPixel) pixels.Left(width, height, i, j);
                pixel.Right = (TreeEvPixel) pixels.Right(width, height, i, j);
                pixel.X = i;
                pixel.Y = j;
            });
        }

        private static PixelType GetType(int width, int height, int x, int y)
        {
            if (y == 0 || y == width - 1)
            {
                return PixelType.Wall;
            }

            return PixelType.Empty;
        }
    }
}