using Genesis.GameWorld;
using VContainer;

namespace Geneses.ArtLife
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterArtLifeGenesis(this IContainerBuilder builder, ArtLifeConfig artLifeConfig)
        {
            builder.Register<ArtLifeWorld>(Lifetime.Singleton);
            builder.Register<ArtLifeGenesis>(Lifetime.Singleton).As<IGenesis>();
            builder.RegisterInstance(artLifeConfig).As<IArtLifeConfig>();
            return builder;
        }
    }
}