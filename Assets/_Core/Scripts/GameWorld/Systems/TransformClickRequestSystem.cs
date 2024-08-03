using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Genesis.GameWorld.Requests;
using UnityEngine;

namespace Genesis.GameWorld.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class TransformClickRequestSystem : UpdateSystem
    {
        private Filter _clickRequests;
        private Filter _world;
        
        public override void OnAwake()
        {
            _clickRequests = World.Filter.With<ClickRequest>().Build();
            _world = World.Filter.With<WorldComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var req in _clickRequests)
            {
                ref var cReq = ref req.GetComponent<ClickRequest>();
                var pos = cReq.ClickPosition;
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    pos /= cWorld.PixelSize;
                    ref var cClickEvent = ref world.AddComponent<PixelClickEvent>();
                    var worldX = (int)pos.x;
                    var worldY = (int)pos.y;
                    cClickEvent.Pixel = cWorld.Pixels[worldX][worldY];
                }
            }
        }
    }
}