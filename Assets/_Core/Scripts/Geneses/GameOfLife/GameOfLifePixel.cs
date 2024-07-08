using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.GameOfLife
{
    public class GameOfLifePixel : IPixel
    {
        public bool IsDirty { get; set; }
        public Color32 Color => State == 0 ? UnityEngine.Color.white : UnityEngine.Color.black;
        public int State { get; set; }
    }
}