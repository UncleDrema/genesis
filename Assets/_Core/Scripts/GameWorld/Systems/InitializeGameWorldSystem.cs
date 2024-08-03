using Genesis.Common.Components;
using Genesis.Common.Tags;
using Genesis.Drawing.Requests;
using Genesis.GameWorld.Events;
using Genesis.GameWorld.Requests;

namespace Genesis.GameWorld.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class InitializeGameWorldSystem : UpdateSystem
    {
        private readonly IGenesis _genesis;
        private readonly IGameWorldConfig _config;
        
        private Filter _gameCanvas;
        private Filter _initGameWorld;

        public InitializeGameWorldSystem(IGenesis genesis, IGameWorldConfig config)
        {
            _genesis = genesis;
            _config = config;
        }
        
        public override void OnAwake()
        {
            _gameCanvas = World.Filter
                .With<GameCanvasTag>()
                .Build();
            _initGameWorld = World.Filter
                .With<InitializeGameWorldRequest>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var initWorld in _initGameWorld)
            {
                ref var cWorld = ref initWorld.AddComponent<WorldComponent>();
                cWorld.Width = _config.WorldWidth;
                cWorld.Height = _config.WorldHeight;
                cWorld.PixelSize = _config.PixelSize;
                var width = cWorld.Width;
                var height = cWorld.Height;
                AddInitCanvasRequest(width, height);

                cWorld.Pixels = new IPixel[width][];
                for (int i = 0; i < width; i++)
                {
                    cWorld.Pixels[i] = new IPixel[height];
                }

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var pixel = _genesis.CreatePixel(width, height, i, j);
                        pixel.IsDirty = true;
                        cWorld.Pixels[i][j] = pixel;
                    }
                }
                
                _genesis.PostProcess(ref cWorld);
                initWorld.AddComponent<WorldInitializedEvent>();
            }
        }

        private void AddInitCanvasRequest(int width, int height)
        {
            foreach (var gameCanvas in _gameCanvas)
            {
                ref var cInitCanvas = ref gameCanvas.AddComponent<InitializeCanvasRequest>();
                cInitCanvas.Width = width;
                cInitCanvas.Height = height;
            }
        }
    }
}