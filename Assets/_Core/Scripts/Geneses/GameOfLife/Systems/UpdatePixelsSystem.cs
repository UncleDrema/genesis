using Geneses.GameOfLife.Components;
using Geneses.GameOfLife.Requests;
using Genesis.Common.Components;
using Scellecs.Morpeh;

namespace Geneses.GameOfLife.Systems
{
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class UpdatePixelsSystem : UpdateSystem
    {
        private Filter _world;
        
        public override void OnAwake()
        {
            _world = World.Filter
                .With<WorldComponent>()
                .With<UpdatePixelsComponent>()
                .With<UpdatePixelsRequest>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var world in _world)
            {
                ref var cWorld = ref world.GetComponent<WorldComponent>();
                ref var cUpdate = ref world.GetComponent<UpdatePixelsComponent>();
                var pixels = cWorld.Pixels;
                foreach (var update in cUpdate.PixelUpdates)
                {
                    var pixel = (GameOfLifePixel) pixels[update.x][update.y];
                    pixel.State = update.newState;
                    pixel.IsDirty = true;
                }
            }
        }
    }
}