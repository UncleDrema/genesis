using Genesis.GameWorld;
using VContainer;

namespace Geneses.Blank
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterBlankGenesis(this IContainerBuilder builder)
        {
            builder.RegisterInstance(new BlankGenesis()).As<IGenesis>();
            return builder;
        }
    }
}