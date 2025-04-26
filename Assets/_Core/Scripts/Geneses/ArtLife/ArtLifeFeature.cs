using Geneses.ArtLife.Requests;
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
            RegisterRequest<CreatePresetCellRequest>();
            RegisterRequest<UpdateCurrentViewRequest>();
            RegisterRequest<DisplayCellInfoRequest>();
            RegisterRequest<UpdateDisplayRequest>();
            RegisterRequest<ClearOrganicRequest>();
            RegisterRequest<SetToolRequest>();
            RegisterRequest<SetDrawModeRequest>();
            
            AddSystem(new ResetArtLifeSystem());
            AddSystem(new InitializeArtLifeSystem(_artLifeWorld));
            AddSystem(new UpdateClickedPixelsSystem());
            AddSystem(new UpdateWorldSystem());
            
            AddSystem(new DisplayCellInfoSystem());
            AddSystem(new UpdateArtLifeDisplay());
        }
    }
}