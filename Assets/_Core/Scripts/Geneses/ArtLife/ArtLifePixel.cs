using System;
using UnityEngine;
using Genesis.GameWorld;
using JetBrains.Annotations;

namespace Geneses.ArtLife
{
    public class ArtLifePixel : IPixel
    {
        public ArtLifeWorld World { get; set; }
        
        public bool IsDirty { get; set; }
        
        public int X { get; set; }
        
        public int Y { get; set; }

        public Color32 Color
        {
            get => GetColor();
        }

        private Color GetColor()
        {
            if (World.Config.DrawMode == DrawMode.Default)
            {
                return GetDefaultColor();
            }
            else
            {
                return UnityEngine.Color.black;
            }
        }

        private Color GetDefaultColor()
        {
            Color color;
            switch (Content)
            {
                case PixelContentType.Empty:
                    color = GetEmptyColor();
                    break;
                case PixelContentType.Cell:
                    color = Cell!.GetColor();
                    break;
                case PixelContentType.Wall:
                    color = UnityEngine.Color.black;
                    break;
                case PixelContentType.Organic:
                    color = UnityEngine.Color.grey;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var radiationFactor = Mathf.Clamp01(RadiationLevel);
            color.r = Mathf.Lerp(color.r, 1f, radiationFactor);
            return color;
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
        public float RadiationLevel { get; set; } = 0;

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
