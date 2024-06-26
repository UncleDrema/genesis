using VContainer;

namespace Genesis.SceneManagement
{
    public static class LifetimeScopeExtension
    {
        public static IContainerBuilder RegisterSceneManagement(this IContainerBuilder builder,
                                                                SceneRepository sceneRepository)
        {
            builder.RegisterInstance(sceneRepository);
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Scoped);
            
            return builder;
        }
    }
}