using Geneses.ArtLife;
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
        
        [SerializeField]
        private GameWorldConfigAsset _gameWorldConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterSceneManagement(_sceneRepository);
            builder.RegisterGameWorld(_gameWorldConfig);
            builder.RegisterArtLifeGenesis();
        }
    }
}