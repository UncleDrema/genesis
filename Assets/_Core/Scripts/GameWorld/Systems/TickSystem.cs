using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Genesis.GameWorld.Tags;

namespace Genesis.GameWorld.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class TickSystem : UpdateSystem
    {
        private Filter _ticks;
        
        public override void OnAwake()
        {
            _ticks = World.Filter
                .With<TickComponent>()
                .Without<PausedTag>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var tick in _ticks)
            {
                ref var cTick = ref tick.GetComponent<TickComponent>();
                cTick.Timer -= deltaTime;
                if (cTick.Timer < 0)
                {
                    tick.AddComponent<TickEvent>();
                    cTick.Timer = cTick.Cooldown;
                }
            }
        }
    }
}