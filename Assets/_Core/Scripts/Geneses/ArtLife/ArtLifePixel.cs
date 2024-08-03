using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.ArtLife
{
    public class ArtLifePixel : IPixel
    {
        public bool IsDirty { get; set; }
        public Color32 Color => UnityEngine.Color.black;
    }
}