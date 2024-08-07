﻿using Genesis.Common.Components;
using Genesis.GameWorld.Events;

namespace Geneses.GameOfLife.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class InitializeGameOfLifeSystem : UpdateSystem
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
            }
        }
    }
}