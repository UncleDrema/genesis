using Genesis.GameWorld.Events;
using Scellecs.Morpeh;

namespace Geneses.GameOfLife.Systems
{
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class UpdateClickedPixelsSystem : UpdateSystem
    {
        private Filter _clickedPixels;
        
        public override void OnAwake()
        {
            _clickedPixels = World.Filter.With<PixelClickedEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var ev in _clickedPixels)
            {
                ref var cPixelClicked = ref ev.GetComponent<PixelClickedEvent>();
                var pixel = (GameOfLifePixel)cPixelClicked.Pixel;
                pixel.State = 1 - pixel.State;
                pixel.IsDirty = true;
            }
        }
    }
}