using Genesis.Common.Components;
using Genesis.GameWorld.Events;

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
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 2; j < height; j++)
                        {
                            var pixel = (TreeEvPixel) pixels[i][j];
                            if (pixel.Type == PixelType.Fruit && pixel.Gene == -2)
                            {
                                pixel.Type = PixelType.Seed;
                                pixel.IsDirty = true;
                            }
                            else if (pixel.Type == PixelType.Seed && pixel.Under.Type == PixelType.Empty)
                            {
                                pixel.Type = PixelType.Empty;
                                pixel.IsDirty = true;
                                pixel.Under.Type = PixelType.Seed;
                                pixel.Under.IsDirty = true;
                                // TODO: передвинуть генетический код из пикселя вниз и очистить в предыдущей клетке
                            }
                            else if (pixel.Type == PixelType.Seed && pixel.Under.Type != PixelType.Wall)
                            {
                                pixel.Type = PixelType.Empty;
                                pixel.IsDirty = true;
                            }
                        }
                    }
                }
            }
        }
    }
}