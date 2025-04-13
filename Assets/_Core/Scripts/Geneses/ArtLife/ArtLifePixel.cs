using UnityEngine;
using Genesis.GameWorld;
using JetBrains.Annotations;

namespace Geneses.ArtLife
{
    public class ArtLifePixel : IPixel
    {
        public bool IsDirty { get; set; }
        public Color32 Color => Cell?.GetColor() ?? GetNonCellColor();

        private Color GetNonCellColor()
        {
            var greenFactor = Mathf.Clamp01(PhotosynthesisEnergy / 10f);
            var mineralFactor = Mathf.Clamp01(MineralCount / 200f);
            var redFactor = Mathf.Clamp01(OrganicCount / 100f);
            
            var color = new Color(redFactor, greenFactor, mineralFactor);
            return color;
        }

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
