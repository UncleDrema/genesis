using Genesis.Common.Components;
using Genesis.GameWorld;
using UnityEngine;

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
            if (y == 0 || y == width - 1 || Distance((float)width / 2, 0, x, y * 2) < 128)
            {
                return PixelType.Wall;
            }

            return PixelType.Empty;
        }

        private static float Distance(float x0, float y0, float x1, float y1)
        {
            return Vector2.Distance(new Vector2(x0, y0), new Vector2(x1, y1));
        }
    }
}