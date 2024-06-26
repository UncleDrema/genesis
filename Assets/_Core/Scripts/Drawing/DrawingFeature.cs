using Genesis.Drawing.Requests;
using Genesis.Drawing.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Genesis.Drawing
{
    public class DrawingFeature : UpdateFeature
    {
        protected override void Initialize()
        {
            RegisterRequest<ApplyTextureChangesRequest>();
            RegisterRequest<ClearCanvasRequest>();
            RegisterRequest<InitializeCanvasRequest>();
            RegisterRequest<SetPixelsRequest>();
            
            AddSystem(new InitializeCanvasSystem());
            AddSystem(new DrawSystem());
            AddSystem(new ApplyTextureChangesSystem());
        }
    }
}