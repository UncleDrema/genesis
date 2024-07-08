using System.Collections.Generic;
using Geneses.GameOfLife.Components;
using Geneses.GameOfLife.Requests;
using Genesis.Common.Components;
using Genesis.GameWorld;
using Genesis.GameWorld.Events;
using UnityEngine;

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
                .With<UpdatePixelsComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var tick in _ticks)
            {
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    ref var cUpdate = ref world.GetComponent<UpdatePixelsComponent>();
                    world.AddComponent<UpdatePixelsRequest>();
                    var updatesList = cUpdate.PixelUpdates;
                    updatesList.Clear();
                    var pixels = cWorld.Pixels;
                    var width = cWorld.Width;
                    var height = cWorld.Height;
                    
                    cWorld.ForEach<GameOfLifePixel>((i, j, pixel) =>
                    {
                        var neighbours = 0;
                        var prevState = pixel.State;
                        foreach (var (dx, dy) in MooreOffsets)
                        {
                            var neighbour = (GameOfLifePixel)pixels.GetSafe(width, height, i + dx, j + dy);
                            if (neighbour.State > 0)
                            {
                                neighbours++;
                            }
                        }
                        
                        int newState;
                        if (prevState is 0)
                        {
                            if (neighbours is 3 or 5)
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
                            if (neighbours is 2 or 3 or 4 or 5)
                            {
                                newState = 1;
                            }
                            else
                            {
                                newState = 0;
                            }
                        }

                        if (prevState != newState)
                        {
                            updatesList.Add(new PixelUpdate()
                            {
                                x = i,
                                y = j,
                                newState = newState
                            });
                        }
                    });
                }
            }
        }
    }
}