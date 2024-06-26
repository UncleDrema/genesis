using UnityEngine;

namespace Genesis.GameWorld
{
    public interface IPixel
    {
        public bool IsDirty { get; set; }
        
        public Color32 Color { get; }
    }
}