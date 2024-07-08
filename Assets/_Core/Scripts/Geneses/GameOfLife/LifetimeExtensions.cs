using Genesis.GameWorld;
using VContainer;

namespace Geneses.GameOfLife
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterGameOfLifeGenesis(this IContainerBuilder builder)
        {
            builder.RegisterInstance(new GameOfLifeGenesis()).As<IGenesis>();
            return builder;
        }
    }
}