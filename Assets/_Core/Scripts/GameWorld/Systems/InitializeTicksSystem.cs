﻿using Genesis.Common.Components;

namespace Genesis.GameWorld.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class InitializeTicksSystem : Initializer
    {
        private readonly IGameWorldConfig _config;

        public InitializeTicksSystem(IGameWorldConfig config)
        {
            _config = config;
        }
        
        public override void OnAwake()
        {
            var e = World.CreateEntity();
            ref var cTimer = ref e.AddComponent<TickComponent>();
            cTimer.Timer = _config.TickPeriod;
        }
    }
}