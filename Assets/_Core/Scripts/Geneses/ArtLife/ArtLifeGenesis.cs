﻿using Geneses.ArtLife.ConstructingLife;
using Genesis.Common.Components;
using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.ArtLife
{
    public class ArtLifeGenesis : IGenesis
    {
        private readonly ArtLifeWorld _world;

        public ArtLifeGenesis(ArtLifeWorld world)
        {
            _world = world;
        }

        public IPixel CreatePixel(int width, int height, int x, int y)
        {
            var pixel = new ArtLifePixel();

            if (x == width / 2 && y == height - height / 3)
            {
                _world.CreateCell(pixel).FillFromSource(LifePresets.SimpleLife(), 255);
            }
            
            return pixel;
        }

        private static readonly Vector2Int[] Directions =
        {
            new(1, 0), new(1, 1), new(0, 1), new(-1, 1),
            new(-1, 0), new(-1, -1), new(0, -1), new(1, -1)
        };
        
        public void PostProcess(ref WorldComponent cWorld)
        {
            // Проставим соседей для каждого пикселя от 0 (вправо) до 7 (право-вниз)
            for (int x = 0; x < cWorld.Width; x++)
            {
                for (int y = 0; y < cWorld.Height; y++)
                {
                    var pixel = (ArtLifePixel)cWorld.Pixels[x][y];
                    pixel.X = x;
                    pixel.Y = y;

                    for (int i = 0; i < 8; i++)
                    {
                        var neighbour = (ArtLifePixel) cWorld.GetSafe(x + Directions[i].x, y + Directions[i].y);
                        pixel.Neighbors[i] = neighbour;
                    }
                }
            }
        }
    }
}