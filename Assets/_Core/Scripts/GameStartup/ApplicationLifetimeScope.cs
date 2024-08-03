using Geneses.GameOfLife;
using Geneses.TreeEv;
using Genesis.GameWorld;
using Genesis.SceneManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Genesis.GameStartup
{
    internal class ApplicationLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private SceneRepository _sceneRepository;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterSceneManagement(_sceneRepository);
            builder.RegisterGameWorld(new GameWorldConfig());
            builder.RegisterGameOfLifeGenesis();
        }
        
        private class GameWorldConfig : IGameWorldConfig
        {
            public int WorldWidth => 400;
            public int WorldHeight => 300;
            public int PixelSize => 2;
            public float TickPeriod => 1f / 60f;
        }
    }
}