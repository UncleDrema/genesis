using Geneses.ArtLife.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Geneses.ArtLife
{
    public class ArtLifeFeature : UpdateFeature
    {
        protected override void Initialize()
        {
            AddSystem(new InitializeArtLifeSystem());
            AddSystem(new UpdateClickedPixelsSystem());
            AddSystem(new UpdateWorldSystem());
        }
    }
}