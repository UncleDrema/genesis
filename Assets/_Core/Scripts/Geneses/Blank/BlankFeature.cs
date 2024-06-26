using Geneses.Blank.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Geneses.Blank
{
    public class BlankFeature : UpdateFeature
    {
        protected override void Initialize()
        {
            AddSystem(new RegenerateBlankSystem());
        }
    }
}