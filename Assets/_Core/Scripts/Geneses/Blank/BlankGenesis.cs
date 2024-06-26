using Genesis.Common.Components;
using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.Blank
{
    public class BlankGenesis : IGenesis
    {
        public IPixel CreatePixel(int width, int height, int x, int y)
        {
            var res = new BlankPixel();
            res.Color = RandColor();
            return res;
        }

        public void PostProcess(ref WorldComponent cWorld) { }

        public static Color32 RandColor()
        {
            return new Color32(RandByte(), RandByte(), RandByte(), 255);
        }

        public static byte RandByte()
        {
            return (byte)Random.Range(0, 255);
        }
    }
}