using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Genesis.GameWorld.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Feature.Events;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Genesis.GameWorld.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetGameWorldSystem : UpdateSystem
    {
        private Filter _worlds;
        private Filter _resetRequests;
        
        public override void OnAwake()
        {
            _worlds = World.Filter.With<WorldComponent>().Build();
            _resetRequests = World.Filter.With<ResetGameWorldRequest>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var _ in _resetRequests)
            {
                foreach (var world in _worlds)
                {
                    world.RemoveComponent<WorldComponent>();
                }

                World.CreateEventEntity<WorldResetEvent>();
            }
        }
    }
}