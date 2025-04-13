using Genesis.GameWorld;
using VContainer;

namespace Geneses.ArtLife
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterArtLifeGenesis(this IContainerBuilder builder)
        {
            builder.Register<ArtLifeWorld>(Lifetime.Singleton);
            builder.Register<ArtLifeGenesis>(Lifetime.Singleton).As<IGenesis>();
            return builder;
        }
    }
}