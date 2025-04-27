using Geneses.ArtLife.Components;
using Geneses.ArtLife.Requests;
using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Geneses.ArtLife.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateWorldSystem : UpdateSystem
    {
        private Filter _world;
        private Filter _ticks;
        private Filter _clearOrganicRequests;
        private Filter _setToolRequests;
        private Filter _setDrawModeRequests;
        private Filter _setSpawningCellRequests;
        private Filter _setToolSizeRequests;
        
        public override void OnAwake()
        {
            _ticks = World.Filter
                .With<TickEvent>()
                .Build();
            _clearOrganicRequests = World.Filter
                .With<ClearOrganicRequest>()
                .Build();
            _world = World.Filter
                .With<WorldComponent>()
                .With<ArtLifeWorldComponent>()
                .Build();
            _setToolRequests = World.Filter
                .With<SetToolRequest>()
                .Build();
            _setDrawModeRequests = World.Filter
                .With<SetDrawModeRequest>()
                .Build();
            _setSpawningCellRequests = World.Filter
                .With<SetSpawningCellRequest>()
                .Build();
            _setToolSizeRequests = World.Filter
                .With<SetToolSizeRequest>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var _ in _ticks)
            {
                foreach (var world in _world)
                {
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    cArtLifeWorld.ArtLifeWorld.Tick();
                }
            }
            
            foreach (var request in _clearOrganicRequests)
            {
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    cArtLifeWorld.ArtLifeWorld.ClearOrganic(ref cWorld);
                }
            }
            
            foreach (var request in _setToolRequests)
            {
                foreach (var world in _world)
                {
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    ref var cSetToolRequest = ref request.GetComponent<SetToolRequest>();
                    cArtLifeWorld.ArtLifeWorld.Tool = cSetToolRequest.Tool;
                }
            }
            
            foreach (var request in _setDrawModeRequests)
            {
                foreach (var world in _world)
                {
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    
                    ref var cSetDrawModeRequest = ref request.GetComponent<SetDrawModeRequest>();
                    cArtLifeWorld.ArtLifeWorld.DrawMode = cSetDrawModeRequest.DrawMode;
                    SetWorldDirty(ref cWorld);
                }
            }
            
            foreach (var request in _setSpawningCellRequests)
            {
                foreach (var world in _world)
                {
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    ref var cSetSpawningCellRequest = ref request.GetComponent<SetSpawningCellRequest>();
                    cArtLifeWorld.ArtLifeWorld.SpawningCellGenome = cSetSpawningCellRequest.SpawningCellGenome;
                }
            }
            
            foreach (var request in _setToolSizeRequests)
            {
                foreach (var world in _world)
                {
                    ref var cArtLifeWorld = ref world.GetComponent<ArtLifeWorldComponent>();
                    
                    ref var cSetToolSizeRequest = ref request.GetComponent<SetToolSizeRequest>();
                    cArtLifeWorld.ArtLifeWorld.ToolSize = cSetToolSizeRequest.ToolSize;
                }
            }
        }
        
        private void SetWorldDirty(ref WorldComponent world)
        {
            var width = world.Width;
            var height = world.Height;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    world.Pixels[x][y].IsDirty = true;
                }
            }
        }
    }
}