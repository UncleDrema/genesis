using Geneses.ArtLife.Components;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Geneses.ArtLife.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetArtLifeSystem : UpdateSystem
    {
        private Filter _worldResetEvents;
        private Filter _artLifeWorlds;
        
        public override void OnAwake()
        {
            _worldResetEvents = World.Filter.With<WorldResetEvent>().Build();
            _artLifeWorlds = World.Filter.With<ArtLifeWorldComponent>().With<ArtLifeDisplayComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var _ in _worldResetEvents)
            {
                foreach (var artLifeWorld in _artLifeWorlds)
                {
                    ref var cWorld = ref artLifeWorld.GetComponent<ArtLifeWorldComponent>();
                    cWorld.ArtLifeWorld.ResetWorld();
                    artLifeWorld.RemoveComponent<ArtLifeWorldComponent>();
                    artLifeWorld.RemoveComponent<ArtLifeDisplayComponent>();
                }
            }
        }
    }
}