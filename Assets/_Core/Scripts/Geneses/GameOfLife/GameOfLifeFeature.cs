using Geneses.GameOfLife.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Geneses.GameOfLife
{
    public class GameOfLifeFeature : UpdateFeature
    {
        protected override void Initialize()
        {
            AddSystem(new InitializeGameOfLifeSystem());
            AddSystem(new UpdateClickedPixelsSystem());
            AddSystem(new GameOfLifeSystem());
            AddSystem(new UpdatePixelsSystem());
        }
    }
}