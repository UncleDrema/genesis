using System;
using UnityEngine;
using Genesis.GameWorld;
using JetBrains.Annotations;

namespace Geneses.ArtLife
{
    public class ArtLifePixel : IPixel
    {
        public bool IsDirty { get; set; }

        public Color32 Color
        {
            get
            {
                switch (Content)
                {
                    case PixelContentType.Empty:
                        return GetEmptyColor();
                        break;
                    case PixelContentType.Cell:
                        return Cell!.GetColor();
                        break;
                    case PixelContentType.Wall:
                        return UnityEngine.Color.black;
                        break;
                    case PixelContentType.Organic:
                        return UnityEngine.Color.grey;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Color GetEmptyColor()
        {
            var greenFactor = Mathf.Clamp01(PhotosynthesisEnergy / 10f);
            var mineralFactor = Mathf.Clamp01(MineralCount / 50f);
            var redFactor = 0f;
            
            var color = new Color(redFactor, greenFactor, mineralFactor);
            return color;
        }

        public bool IsEmpty => Content == PixelContentType.Empty;
        
        [CanBeNull]
        public ArtLifeCell Cell { get; private set; }

        public PixelContentType Content { get; set; } = PixelContentType.Empty;
        
        public ArtLifePixel[] Neighbors { get; set; } = new ArtLifePixel[8];
        
        public int PhotosynthesisEnergy { get; set; } = 0;
        
        public int MineralCount { get; set; } = 0;
        
        public void SetCell(ArtLifeCell cell)
        {
            Content = PixelContentType.Cell;
            Cell = cell;
            IsDirty = true;
        }
        
        public void MakeEmpty()
        {
            Content = PixelContentType.Empty;
            Cell = null;
            IsDirty = true;
        }
        
        public void MakeWall()
        {
            Content = PixelContentType.Wall;
            Cell = null;
            IsDirty = true;
        }
        
        public void MakeOrganic()
        {
            Content = PixelContentType.Organic;
            Cell = null;
            IsDirty = true;
        }
    }
}
