using System;
using System.Collections.Generic;
using Genesis.Common.Components;
using UnityEngine;

namespace Geneses.ArtLife
{
    public class ArtLifeWorld
    {
        private readonly IArtLifeConfig _config;
        public IArtLifeConfig Config => _config;
        
        public ArtLifeWorld(IArtLifeConfig config)
        {
            _config = config;
        }
        
        private readonly CellList _cellList = new();

        public ArtLifeCell CreateCell(ArtLifePixel pixel)
        {
            var cell = new ArtLifeCell(this, pixel);
            _cellList.Add(cell);
            pixel.SetCell(cell);
            return cell;
        }
        
        public void RemoveCell(ArtLifeCell cell)
        {
            var node = cell.Node;
            _cellList.Remove(node);
            cell.Position.MakeEmpty();
            cell.Node = null;
            cell.Position = null;
        }

        public void Tick()
        {
            var zero = _cellList.Zero;
            var current = zero.Next;
            while (current != zero)
            {
                var cell = current.Cell;
                var next = current.Next;
                // Клетка может удалить себя из списка
                cell.Tick();
                current = next;
            }
        }

        public void UpdatePixels(ref WorldComponent cWorld)
        {
            var width = cWorld.Width;
            var height = cWorld.Height;
            
            var photosynthesisLevel = _config.PhotosynthesisLevel;
            var photosynthesisEnergyMax = _config.PhotosynthesisEnergyMax;
            var minPhotosynthesisHeight = height * (1 - photosynthesisLevel);
            
            var mineralsLevel = _config.MineralsLevel;
            var mineralsPerLayer = _config.MineralsPerLayer;
            var mineralLayersCount = _config.MineralLayersCount;
            
            var maxMineralsHeight = height * mineralsLevel;
            var layerWidth = maxMineralsHeight / mineralLayersCount;
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pixel = (ArtLifePixel) cWorld.Pixels[x][y];

                    if (y >= minPhotosynthesisHeight)
                    {
                        var photoSynthesisMult = Remap(y, height, minPhotosynthesisHeight, 1, 0);
                        var photosynthesisEnergy = (int)(photoSynthesisMult * photosynthesisEnergyMax);
                        pixel.PhotosynthesisEnergy = photosynthesisEnergy;
                    }
                    else
                    {
                        pixel.PhotosynthesisEnergy = 0;
                    }
                    
                    if (y <= maxMineralsHeight)
                    {
                        var mineralNumber = (int)((maxMineralsHeight - y) / layerWidth) + 1;
                        var mineralsCount = mineralsPerLayer * mineralNumber;
                        pixel.MineralCount = mineralsCount;
                    }
                    else
                    {
                        pixel.MineralCount = 0;
                    }
                }
            }
        }
        
        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return from2 + (to2 - from2) * ((value - from1) / (to1 - from1));
        }
    }
}