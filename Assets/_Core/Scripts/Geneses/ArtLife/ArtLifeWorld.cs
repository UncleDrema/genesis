using System;
using System.Collections.Generic;
using Geneses.ArtLife.ConstructingLife;
using Genesis.Common.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Geneses.ArtLife
{
    public class ArtLifeWorld
    {
        private readonly IArtLifeConfig _config;
        public IArtLifeConfig Config => _config;
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ToolType Tool { get; set; } = ToolType.Clear;
        public DrawMode DrawMode { get; set; } = DrawMode.EnergySource;
        public byte[] SpawningCellGenome { get; set; } = LifePresets.SimpleLife();
        public int ToolSize { get; set; }
        public int MaxAge { get; private set; }
        public int MaxMutations { get; private set; }

        public ArtLifeWorld(IArtLifeConfig config)
        {
            _config = config;
        }
        
        private CellList _cellList = new();

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
                var prev = current.Prev;
                var cell = current.Cell;
                cell.Tick();
                if (cell.Node == null)
                {
                    // Cell was removed
                    current = prev.Next;
                }
                else
                {
                    // Cell is still alive
                    current = current.Next;
                }
            }
            UpdateStats();
        }

        private void UpdateStats()
        {
            var zero = _cellList.Zero;
            var current = zero.Next;
            var maxAge = 0;
            var maxMutations = 0;
            while (current != zero)
            {
                var cell = current.Cell;
                if (cell.Age > maxAge)
                {
                    maxAge = cell.Age;
                }
                if (cell.TotalMutations > maxMutations)
                {
                    maxMutations = cell.TotalMutations;
                }
                current = current.Next;
            }
            MaxAge = maxAge;
            MaxMutations = maxMutations;
        }

        public void InitWorld(ref WorldComponent cWorld)
        {
            var width = cWorld.Width;
            var height = cWorld.Height;
            Width = width;
            Height = height;
            
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
                    pixel.World = this;
                    
                    var worldCenter = new Vector2(width / 2, height / 2 - height / 4);
                    var pos = new Vector2(x, y);
                    var diff = (worldCenter - pos);
                    diff.x /= 4;
                    diff.y *= 3;
                    var distanceToCenter = diff.magnitude;
                    if (distanceToCenter <= _config.RadiationRadius)
                    {
                        var radiationLevel = Mathf.Clamp01(1 - distanceToCenter / _config.RadiationRadius);
                        pixel.RadiationLevel = radiationLevel;
                    }

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

        public void ResetWorld()
        {
            _cellList = new CellList();
            Width = 0;
            Height = 0;
        }

        public void ClearOrganic(ref WorldComponent cWorld)
        {
            var width = cWorld.Width;
            var height = cWorld.Height;
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pixel = (ArtLifePixel) cWorld.Pixels[x][y];
                    if (pixel.Content is PixelContentType.Organic)
                    {
                        pixel.MakeEmpty();
                    }
                }
            }
        }

        public List<ArtLifePixel> GetPixelsInRange(ref WorldComponent cWorld, ArtLifePixel pixel, int toolSize)
        {
            // Вернем квадрат с центром в pixel размером toolSize
            var pixels = new List<ArtLifePixel>();
            var x = pixel.X;
            var y = pixel.Y;
            var width = cWorld.Width;
            var height = cWorld.Height;
            var halfSize = toolSize / 2;
            var startX = Mathf.Max(0, x - halfSize);
            var startY = Mathf.Max(0, y - halfSize);
            var endX = Mathf.Min(width - 1, x + halfSize);
            var endY = Mathf.Min(height - 1, y + halfSize);
            for (var i = startX; i <= endX; i++)
            {
                for (var j = startY; j <= endY; j++)
                {
                    var pixelInRange = (ArtLifePixel) cWorld.Pixels[i][j];
                    pixels.Add(pixelInRange);
                }
            }
            return pixels;
        }
    }
}