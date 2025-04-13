using Geneses.ArtLife.Components;
using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Geneses.ArtLife.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateClickedPixelsSystem : UpdateSystem
    {
        private Filter _clickedPixels;
        private Filter _world;
        
        public override void OnAwake()
        {
            _clickedPixels = World.Filter.With<PixelClickedEvent>().Build();
            _world = World.Filter.With<WorldComponent>().With<ArtLifeWorldComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var ev in _clickedPixels)
            {
                ref var cPixelClicked = ref ev.GetComponent<PixelClickedEvent>();
                var pixel = (ArtLifePixel)cPixelClicked.Pixel;
                
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    // Если в пикселе уже есть клетка, то ничего не делаем
                    if (pixel.Cell != null)
                        continue;

                    // Создаем клетку и заполняем ее геномом
                    var cell = cArtLifeWorld.ArtLifeWorld.CreateCell(pixel);
                    cell.FillGenomeWithValue(0);
                }
            }
        }
    }
}