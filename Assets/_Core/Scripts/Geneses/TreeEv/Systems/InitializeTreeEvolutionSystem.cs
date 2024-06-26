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
    internal sealed class InitializeTreeEvolutionSystem : UpdateSystem
    {
        private Filter _initializedWorld;
        
        public override void OnAwake()
        {
            _initializedWorld = World.Filter
                .With<WorldComponent>()
                .With<WorldInitializedEvent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var world in _initializedWorld)
            {
                ref var cWorld = ref world.GetComponent<WorldComponent>();
                var count = 3;
                var step = cWorld.Width / (count + 1);
                for (int i = 1; i <= count; i++)
                {
                    MakeSeed((TreeEvPixel) cWorld.Pixels[step * i][cWorld.Height - 2]);
                }
            }
        }

        private void MakeSeed(TreeEvPixel pixel)
        {
            pixel.Type = PixelType.Seed;
            pixel.IsDirty = true;
        }
    }
}