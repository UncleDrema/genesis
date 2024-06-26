using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Genesis.SceneManagement
{
    public class SceneLoader : ISceneLoader
    {
        private readonly LifetimeScope _currentScope;
        private readonly SceneRepository _sceneRepository;
        
        public SceneLoader(LifetimeScope currentScope, SceneRepository sceneRepository)
        {
            _currentScope = currentScope;
            _sceneRepository = sceneRepository;
        }

        public async UniTask LoadTestScene()
        {
            await LoadSceneAsync(_sceneRepository.GameScenePath);
        }

        private async UniTask LoadSceneAsync(string scenePath)
        {
            using (LifetimeScope.EnqueueParent(_currentScope))
            {
                var handler = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
                handler.completed += _ =>
                {
                    var loadedScene = SceneManager.GetSceneByPath(scenePath);
                    SceneManager.SetActiveScene(loadedScene);
                };

                await handler;
            }
        }
    }
}