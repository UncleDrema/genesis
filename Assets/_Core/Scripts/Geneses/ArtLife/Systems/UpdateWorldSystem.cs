using Geneses.ArtLife.Components;
using Geneses.ArtLife.Requests;
using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Geneses.ArtLife.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateWorldSystem : UpdateSystem
    {
        private Filter _world;
        private Filter _ticks;
        private Filter _clearOrganicRequests;
        
        public override void OnAwake()
        {
            _ticks = World.Filter
                .With<TickEvent>()
                .Build();
            _clearOrganicRequests = World.Filter
                .With<ClearOrganicRequest>()
                .Build();
            _world = World.Filter
                .With<WorldComponent>()
                .With<ArtLifeWorldComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var _ in _ticks)
            {
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    //cArtLifeWorld.ArtLifeWorld.UpdatePixels(ref cWorld);
                    cArtLifeWorld.ArtLifeWorld.Tick();
                }
            }
            
            foreach (var request in _clearOrganicRequests)
            {
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    cArtLifeWorld.ArtLifeWorld.ClearOrganic(ref cWorld);
                }
            }
        }
    }
}