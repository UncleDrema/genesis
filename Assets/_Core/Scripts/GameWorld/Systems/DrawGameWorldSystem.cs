using Genesis.Common.Components;
using Genesis.Common.Tags;
using Genesis.Drawing;

namespace Genesis.GameWorld.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class DrawGameWorldSystem : LateUpdateSystem
    {
        private Filter _gameCanvas;
        private Filter _gameWorld;
        
        public override void OnAwake()
        {
            _gameCanvas = World.Filter
                .With<GameCanvasTag>()
                .Build();
            _gameWorld = World.Filter
                .With<WorldComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var canvas in _gameCanvas)
            {
                foreach (var gameWorld in _gameWorld)
                {
                    ref var cGameWorld = ref gameWorld.GetComponent<WorldComponent>();
                    for (int i = 0; i < cGameWorld.Width; i++)
                    {
                        for (int j = 0; j < cGameWorld.Height; j++)
                        {
                            var pixel = cGameWorld.Pixels[i][j];
                            if (pixel.IsDirty)
                            {
                                canvas.SetPixel(i, j, pixel.Color);
                                pixel.IsDirty = false;
                            }
                        }
                    }
                }
            }
        }
    }
}