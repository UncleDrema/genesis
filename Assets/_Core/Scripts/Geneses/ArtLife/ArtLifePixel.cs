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
            if (World.DrawMode == DrawMode.EnergySource)
            {
                return EnergySourceColor();
            }
            else if (World.DrawMode == DrawMode.Energy)
            {
                return EnergyColor();
            }
            else if (World.DrawMode == DrawMode.Age)
            {
                return AgeColor();
            }
            else if (World.DrawMode == DrawMode.Mutations)
            {
                return MutationsColor();
            }
            else if (World.DrawMode == DrawMode.Minerals)
            {
                return MineralsColor();
            }
            else
            {
                return UnityEngine.Color.black;
            }
        }

        private Color EnergyColor()
        {
            if (Content != PixelContentType.Cell)
            {
                return GetNonCellColor();
            }

            var energyColorMap = World.Config.EnergyColorMap;
            var maxEnergy = World.Config.MaxEnergy;
            var energyFactor = Mathf.Clamp01((float)Cell!.Energy / maxEnergy);
            return energyColorMap.GetColor(energyFactor);
        }
        
        private Color AgeColor()
        {
            if (Content != PixelContentType.Cell)
            {
                return GetNonCellColor();
            }
            
            var ageColorMap = World.Config.AgeColorMap;
            var maxAge = World.MaxAge;
            var ageFactor = Mathf.Clamp01((float)Cell!.Age / maxAge);
            return ageColorMap.GetColor(ageFactor);
        }
        
        private Color MutationsColor()
        {
            if (Content != PixelContentType.Cell)
            {
                return GetNonCellColor();
            }
            
            var mutationsColorMap = World.Config.MutationsColorMap;
            var maxMutations = World.MaxMutations;
            var mutationsFactor = Mathf.Clamp01((float)Cell!.TotalMutations / maxMutations);
            return mutationsColorMap.GetColor(mutationsFactor);
        }
        
        private Color MineralsColor()
        {
            if (Content != PixelContentType.Cell)
            {
                return GetNonCellColor();
            }
            
            var mineralsColorMap = World.Config.MineralsColorMap;
            var maxMinerals = World.Config.MaxAccumulatedMinerals;
            var mineralsFactor = Mathf.Clamp01(1f - (float)Cell!.AccumulatedMineralsCount / maxMinerals);
            return mineralsColorMap.GetColor(mineralsFactor);
        }

        private Color GetNonCellColor()
        {
            switch (Content)
            {
                case PixelContentType.Empty:
                    return GetEmptyColor();
                case PixelContentType.Wall:
                    return UnityEngine.Color.black;
                case PixelContentType.Organic:
                    return UnityEngine.Color.grey;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Color EnergySourceColor()
        {
            var color = Content == PixelContentType.Cell
                ? Cell!.GetColor()
                : GetNonCellColor();

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
