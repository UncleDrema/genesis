using Genesis.SceneManagement;
using UnityEngine;
using VContainer;

namespace Genesis.GameStartup
{
    internal class GameStartup : MonoBehaviour
    {
        private ISceneLoader _sceneLoader;
        
        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        protected void Awake()
        {
            
        }

        public async void Start()
        {
            await _sceneLoader.LoadTestScene();
        }
    }
}