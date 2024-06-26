using System;
using UnityEngine;

namespace Geneses.TreeEv
{
    public static class PixelTypeExtensions
    {
        public static Color32 ToColor(this PixelType pixelType)
        {
            return pixelType switch
            {
                PixelType.Empty => Color.gray,
                PixelType.Tree => Color.green,
                PixelType.Seed => Color.blue,
                PixelType.Wall => Color.black,
                PixelType.Sprout => Color.yellow,
                PixelType.Fruit => Color.red,
                _ => throw new ArgumentOutOfRangeException(nameof(pixelType), pixelType, null)
            };
        }
    }
}