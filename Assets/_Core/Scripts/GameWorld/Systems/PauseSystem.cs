using Genesis.Common.Components;
using Genesis.GameWorld.Requests;
using Genesis.GameWorld.Tags;

namespace Genesis.GameWorld.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class PauseSystem : UpdateSystem
    {
        private Filter _pauseRequests;
        private Filter _ticker;
        
        public override void OnAwake()
        {
            _pauseRequests = World.Filter.With<PauseRequest>().Build();
            _ticker = World.Filter.With<TickComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var _ in _pauseRequests)
            {
                foreach (var ticker in _ticker)
                {
                    if (ticker.Has<PausedTag>())
                    {
                        ticker.RemoveComponent<PausedTag>();
                    }
                    else
                    {
                        ticker.AddComponent<PausedTag>();
                    }
                }
            }
        }
    }
}