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
    public sealed class InitializeWorldAfterResetSystem : LateUpdateSystem
    {
        private Filter _resetEvents;
        
        public override void OnAwake()
        {
            _resetEvents = World.Filter.With<WorldResetEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var _ in _resetEvents)
            {
                World.CreateEventEntity<InitializeGameWorldRequest>();
            }
        }
    }
}