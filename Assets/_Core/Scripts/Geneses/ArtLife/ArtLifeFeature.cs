using Geneses.ArtLife.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Geneses.ArtLife
{
    public class ArtLifeFeature : UpdateFeature
    {
        private readonly ArtLifeWorld _artLifeWorld;
        
        public ArtLifeFeature(ArtLifeWorld artLifeWorld)
        {
            _artLifeWorld = artLifeWorld;
        }
        
        protected override void Initialize()
        {
            AddSystem(new InitializeArtLifeSystem(_artLifeWorld));
            AddSystem(new UpdateClickedPixelsSystem());
            AddSystem(new UpdateWorldSystem());
        }
    }
}