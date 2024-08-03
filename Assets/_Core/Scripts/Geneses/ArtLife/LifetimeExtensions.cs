using Genesis.GameWorld;
using VContainer;

namespace Geneses.ArtLife
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterArtLifeGenesis(this IContainerBuilder builder)
        {
            builder.RegisterInstance(new ArtLifeGenesis()).As<IGenesis>();
            return builder;
        }
    }
}