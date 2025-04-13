using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Geneses.ArtLife.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateClickedPixelsSystem : UpdateSystem
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
                var pixel = (ArtLifePixel)cPixelClicked.Pixel;
                
            }
        }
    }
}