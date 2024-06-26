using Geneses.TreeEv.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Geneses.TreeEv
{
    public class TreeEvFeature : UpdateFeature
    {
        protected override void Initialize()
        {
            AddSystem(new InitializeTreeEvolutionSystem());
            AddSystem(new SeedGravitySystem());
            AddSystem(new TreeGrowthSystem());
        }
    }
}