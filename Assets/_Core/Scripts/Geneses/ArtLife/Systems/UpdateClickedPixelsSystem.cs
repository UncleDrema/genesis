using Geneses.ArtLife.Components;
using Geneses.ArtLife.ConstructingLife;
using Geneses.ArtLife.Requests;
using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Feature.Events;
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
                World.CreateEventEntity<UpdateDisplayRequest>();
                ref var cPixelClicked = ref ev.GetComponent<PixelClickedEvent>();
                var pixel = (ArtLifePixel)cPixelClicked.Pixel;
                
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    // Если в пикселе уже есть клетка, то ничего не делаем
                    if (pixel.Cell != null)
                    {
                        ref var cCellInfoRequest = ref World.CreateEventEntity<DisplayCellInfoRequest>();
                        cCellInfoRequest.Cell = pixel.Cell;
                    }
                    else if (pixel.IsEmpty)
                    {
                        ref var cCreatePresetCellRequest = ref World.CreateEventEntity<CreatePresetCellRequest>();
                        cCreatePresetCellRequest.Position = pixel;

                        pixel.MakeWall();
                        //var cell = cArtLifeWorld.ArtLifeWorld.CreateCell(pixel);
                        //cell.FillFromSource(LifePresets.PredatorLife(), 255);
                    }
                }
            }
        }
    }
}