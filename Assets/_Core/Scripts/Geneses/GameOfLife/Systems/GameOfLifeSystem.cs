using System.Collections.Generic;
using Genesis.Common.Components;
using Genesis.GameWorld;
using Genesis.GameWorld.Events;

namespace Geneses.GameOfLife.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class GameOfLifeSystem : UpdateSystem
    {
        private static readonly List<(int, int)> MooreOffsets = new(8)
        {
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, 1),
            (0, -1),
            (1, -1),
            (1, 0),
            (1, 1)
        };
        private Filter _ticks;
        private Filter _world;
        
        public override void OnAwake()
        {
            _ticks = World.Filter
                .With<TickEvent>()
                .Build();
            _world = World.Filter
                .With<WorldComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var tick in _ticks)
            {
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    var pixels = cWorld.Pixels;
                    var width = cWorld.Width;
                    var height = cWorld.Height;

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            var pixel = (GameOfLifePixel)pixels[x][y];
                            var neighbours = 0;
                            var prevState = pixel.State;
                            
                            // Подсчёт соседей - бутылочное горлышко, поэтому сделан не очень красиво
                            // Большинство случаев - в середине, разворачиваем цикл вручную
                            if (x > 0 && x < (width - 1) &&
                                y > 0 && y < (height - 1))
                            {
                                var right = x + 1;
                                var left = x - 1;
                                var up = y + 1;
                                var down = y - 1;
                                neighbours += ((GameOfLifePixel)pixels[right][up]).State > 0 ? 1 : 0;
                                neighbours += ((GameOfLifePixel)pixels[x][up]).State > 0 ? 1 : 0;
                                neighbours += ((GameOfLifePixel)pixels[left][up]).State > 0 ? 1 : 0;
                                
                                neighbours += ((GameOfLifePixel)pixels[right][y]).State > 0 ? 1 : 0;
                                neighbours += ((GameOfLifePixel)pixels[left][y]).State > 0 ? 1 : 0;
                                
                                neighbours += ((GameOfLifePixel)pixels[right][down]).State > 0 ? 1 : 0;
                                neighbours += ((GameOfLifePixel)pixels[x][down]).State > 0 ? 1 : 0;
                                neighbours += ((GameOfLifePixel)pixels[left][down]).State > 0 ? 1 : 0;
                            }
                            // На границах, проверяем координаты безопасно
                            else
                            {
                                foreach (var (dx, dy) in MooreOffsets)
                                {
                                    var neighbour = (GameOfLifePixel)pixels.GetSafe(width, height, x + dx, y + dy);
                                    if (neighbour.State > 0)
                                    {
                                        neighbours++;
                                    }
                                }
                            }
                        
                            int newState;
                            if (prevState is 0)
                            {
                                if (neighbours is 3)
                                {
                                    newState = 1;
                                }
                                else
                                {
                                    newState = 0;
                                }
                            }
                            else
                            {
                                if (neighbours is 2 or 3)
                                {
                                    newState = 1;
                                }
                                else
                                {
                                    newState = 0;
                                }
                            }

                            pixel.NextState = newState;
                        }
                    }
                }
            }
        }
    }
}