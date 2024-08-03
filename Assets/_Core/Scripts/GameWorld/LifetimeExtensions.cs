using VContainer;

namespace Genesis.GameWorld
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterGameWorld(this IContainerBuilder builder, IGameWorldConfig config)
        {
            builder.RegisterInstance(config);
            
            return builder;
        }
    }
}