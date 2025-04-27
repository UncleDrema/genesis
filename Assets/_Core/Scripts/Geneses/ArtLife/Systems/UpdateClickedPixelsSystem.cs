using System;
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
using UnityEngine.EventSystems;

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
                var startingPixel = (ArtLifePixel)cPixelClicked.Pixel;

                if (cPixelClicked.Button == PointerEventData.InputButton.Right)
                {
                    World.CreateEventEntity<UpdateDisplayRequest>();
                    if (startingPixel.Cell != null)
                    {
                        ref var cCellInfoRequest = ref World.CreateEventEntity<DisplayCellInfoRequest>();
                        cCellInfoRequest.Cell = startingPixel.Cell;
                    }
                }
                else if (cPixelClicked.Button == PointerEventData.InputButton.Left)
                {
                    foreach (var world in _world)
                    {
                        ref var cWorld = ref world.GetComponent<WorldComponent>();
                        ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();

                        var toolSize = cArtLifeWorld.ArtLifeWorld.ToolSize;
                        var pixelsInRange =
                            cArtLifeWorld.ArtLifeWorld.GetPixelsInRange(ref cWorld, startingPixel, toolSize);

                        foreach (var pixel in pixelsInRange)
                        {
                            UseTool(pixel, cArtLifeWorld.ArtLifeWorld);
                        }
                    }
                }
            }
        }

        private void UseTool(ArtLifePixel pixel, ArtLifeWorld world)
        {
            switch (world.Tool)
            {
                case ToolType.SpawnCell:
                    SpawnCellTool(pixel, world);
                    break;
                case ToolType.SpawnWall:
                    SpawnWallTool(pixel, world);
                    break;
                case ToolType.SpawnOrganic:
                    SpawnOrganicTool(pixel, world);
                    break;
                case ToolType.Clear:
                    ClearTool(pixel, world);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(world.Tool), world.Tool, null);
            }
        }

        private void ClearTool(ArtLifePixel pixel, ArtLifeWorld world)
        {
            if (!pixel.IsEmpty)
            {
                if (pixel.Cell != null)
                {
                    world.RemoveCell(pixel.Cell);
                }
                pixel.MakeEmpty();
            }
        }

        private void SpawnOrganicTool(ArtLifePixel pixel, ArtLifeWorld world)
        {
            if (pixel.IsEmpty)
            {
                pixel.MakeOrganic();
            }
        }

        private void SpawnWallTool(ArtLifePixel pixel, ArtLifeWorld world)
        {
            if (pixel.IsEmpty)
            {
                pixel.MakeWall();
            }
        }

        private void SpawnCellTool(ArtLifePixel pixel, ArtLifeWorld world)
        {
            if (pixel.IsEmpty)
            {
                var cell = world.CreateCell(pixel);
                cell.FillFromSource(world.SpawningCellGenome, 255);
            }
        }
    }
}