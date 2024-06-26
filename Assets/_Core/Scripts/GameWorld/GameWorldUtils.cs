using System;
using System.Linq.Expressions;
using Genesis.Common.Components;

namespace Genesis.GameWorld
{
    public static class GameWorldUtils
    {
        public static int GetCoordinate(int x, int size)
        {
            x %= size;
            if (x < 0)
                x = size + x;
            return x;
        }

        public static void ForEach<T>(this ref WorldComponent cWorld, Action<int, int, T> callback)
        where T : class, IPixel
        {
            for (int i = 0; i < cWorld.Width; i++)
            {
                for (int j = 0; j < cWorld.Height; j++)
                {
                    callback(i, j, cWorld.Pixels[i][j] as T);
                }
            }
        }
        
        public static IPixel GetSafe(this ref WorldComponent cWorld, int x, int y)
        {
            return cWorld.Pixels.GetSafeGeneric(x, y, cWorld.Width, cWorld.Height);
        }

        public static IPixel Left(this ref WorldComponent cWorld, int x, int y)
        {
            return cWorld.GetSafe(x - 1, y);
        }
        
        public static IPixel Right(this ref WorldComponent cWorld, int x, int y)
        {
            return cWorld.GetSafe(x + 1, y);
        }
        
        public static IPixel Up(this ref WorldComponent cWorld, int x, int y)
        {
            return cWorld.GetSafe(x, y + 1);
        }
        
        public static IPixel Down(this ref WorldComponent cWorld, int x, int y)
        {
            return cWorld.GetSafe(x, y - 1);
        }
        
        public static IPixel GetSafe(this IPixel[][] pixels, int width, int height, int x, int y)
        {
            return pixels.GetSafeGeneric(x, y, width, height);
        }

        public static IPixel Left(this IPixel[][] pixels, int width, int height, int x, int y)
        {
            return pixels.GetSafe(width, height, x - 1, y);
        }
        
        public static IPixel Right(this IPixel[][] pixels, int width, int height, int x, int y)
        {
            return pixels.GetSafe(width, height, x, y);
        }
        
        public static IPixel Up(this IPixel[][] pixels, int width, int height, int x, int y)
        {
            return pixels.GetSafe(width, height, x, y + 1);
        }
        
        public static IPixel Down(this IPixel[][] pixels, int width, int height, int x, int y)
        {
            return pixels.GetSafe(width, height, x, y - 1);
        }

        public static T GetSafeGeneric<T>(this T[][] arr, int x, int y, int width, int height)
        {
            return arr[GetCoordinate(x, width)][GetCoordinate(y, height)];
        }
    }
}