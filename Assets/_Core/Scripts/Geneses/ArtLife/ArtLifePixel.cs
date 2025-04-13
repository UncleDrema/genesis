using UnityEngine;
using Genesis.GameWorld;
using JetBrains.Annotations;

namespace Geneses.ArtLife
{
    public class ArtLifePixel : IPixel
    {
        public bool IsDirty { get; set; }
        public Color32 Color => Cell?.GetColor() ?? UnityEngine.Color.white;
        
        public bool IsEmpty => Cell == null;
        
        [CanBeNull]
        public ArtLifeCell Cell { get; private set; }
        
        public ArtLifePixel[] Neighbors { get; set; } = new ArtLifePixel[8];
        
        public int PhotosynthesisEnergy { get; set; } = 0;
        
        public int MineralCount { get; set; } = 0;
        public int OrganicCount { get; set; } = 0;
        
        public void SetCell(ArtLifeCell cell)
        {
            Cell = cell;
            IsDirty = true;
        }
        
        public void MakeEmpty()
        {
            Cell = null;
            IsDirty = true;
        }
    }
}
