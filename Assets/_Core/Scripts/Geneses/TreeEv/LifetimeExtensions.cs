using Genesis.GameWorld;
using VContainer;

namespace Geneses.TreeEv
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterTreeEvGenesis(this IContainerBuilder builder)
        {
            builder.RegisterInstance(new TreeEvGenesis()).As<IGenesis>();
            return builder;
        }
    }
}