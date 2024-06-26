using Geneses.Blank;
using Geneses.TreeEv;
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
            builder.RegisterTreeEvGenesis();
        }
    }
}