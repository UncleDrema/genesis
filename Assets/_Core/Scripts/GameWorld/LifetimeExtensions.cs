using VContainer;

namespace Genesis.GameWorld
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterGameWorld(this IContainerBuilder builder, GameWorldConfigAsset config)
        {
            builder.RegisterInstance(config).As<IGameWorldConfig>();
            
            return builder;
        }
    }
}