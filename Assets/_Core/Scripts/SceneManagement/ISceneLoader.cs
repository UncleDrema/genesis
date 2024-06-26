using Cysharp.Threading.Tasks;

namespace Genesis.SceneManagement
{
    public interface ISceneLoader
    {
        UniTask LoadTestScene();
    }
}