using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.TreeEv
{
    public class TreeEvPixel : IPixel
    {
        public bool IsDirty { get; set; }
        public Color32 Color => Type.ToColor();
        public PixelType Type { get; set; }
        public int Gene { get; set; }
        public TreeEvPixel Above { get; set; }
        public TreeEvPixel Under { get; set; }
        public TreeEvPixel Left { get; set; }
        public TreeEvPixel Right { get; set; }
    }
}