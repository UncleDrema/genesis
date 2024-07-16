using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;

namespace Geneses.GameOfLife.Systems
{
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class UpdatePixelsSystem : UpdateSystem
    {
        private Filter _world;
        private Filter _ticks;
        
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
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            var pixel = (GameOfLifePixel)pixels[i][j];
                            if (pixel.State != pixel.NextState)
                            {
                                pixel.IsDirty = true;
                                pixel.State = pixel.NextState;
                            }
                        }
                    }
                }
            }
        }
    }
}