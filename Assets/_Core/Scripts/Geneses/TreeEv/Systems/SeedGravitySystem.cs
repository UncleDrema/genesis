using System.Collections.Generic;
using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using UnityEngine;

namespace Geneses.TreeEv.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class SeedGravitySystem : UpdateSystem
    {
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
                    var height = cWorld.Height;
                    var width = cWorld.Width;
                    var pixels = cWorld.Pixels;
                    for (int i = width - 1; i >= 0; i--)
                    {
                        for (int j = 2; j < height; j++)
                        {
                            var pixel = (TreeEvPixel) pixels[i][j];
                            if (pixel.Type is PixelType.Fruit && pixel.Gene is -2)
                            {
                                pixel.Type = PixelType.Seed;
                            }
                            else if (pixel.Type is PixelType.Seed)
                            {
                                // fall if empty under seed
                                if (pixel.Under.Type is PixelType.Empty)
                                {
                                    pixel.Under.GeneticCode = pixel.GeneticCode;
                                    pixel.Under.Type = PixelType.Seed;
                                    pixel.Type = PixelType.Empty;
                                }
                                // merge into under seed if not empty
                                else if (pixel.Under.Type is PixelType.Seed)
                                {
                                    pixel.Under.GeneticCode = Combine(pixel.GeneticCode, pixel.Under.GeneticCode);
                                    pixel.Type = PixelType.Empty;
                                }
                                // if not stopped lie
                                else if (pixel.Under.Type is not PixelType.Wall)
                                {
                                    if (false)
                                    {
                                        if (!pixel.IsSiblingOf(pixel.Under))
                                        {
                                            pixel.Type = PixelType.Empty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<int, int[]> Combine(Dictionary<int, int[]> a, Dictionary<int, int[]> b)
        {
            var res = new Dictionary<int, int[]>();
            for (int i = 0; i < 32; i++)
            {
                var arr = new int[4] { 0, 0, 0, 0 };
                res[i] = arr;
                for (int j = 0; j < 4; j++)
                {
                    if (Random.value < 0.995f)
                    {
                        arr[j] = Random.value < 0.5f ? a[i][j] : b[i][j];
                    }
                    else
                    {
                        arr[j] = (int)(Random.value * 64);
                    }
                }
            }
            return res;
        }
    }
}