using Genesis.Common.Components;
using Genesis.GameWorld;
using Genesis.GameWorld.Events;

namespace Geneses.Blank.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed  class RegenerateBlankSystem : UpdateSystem
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
            foreach (var _ in _ticks)
            {
                Regenerate();
            }
        }

        private void Regenerate()
        {
            foreach (var world in _world)
            {
                ref var cWorld = ref world.GetComponent<WorldComponent>();
                cWorld.ForEach<BlankPixel>((_, _, pixel) =>
                {
                    pixel.Color = BlankGenesis.RandColor();
                    pixel.IsDirty = true;
                });
            }
        }
    }
}