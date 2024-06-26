using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.Blank
{
    public class BlankPixel : IPixel
    {
        public bool IsDirty { get; set; }
        public Color32 Color { get; set; }
    }
}