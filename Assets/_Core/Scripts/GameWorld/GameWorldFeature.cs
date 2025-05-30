﻿using Genesis.GameWorld.Events;
using Genesis.GameWorld.Requests;
using Genesis.GameWorld.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Genesis.GameWorld
{
    public class GameWorldFeature : UpdateFeature
    {
        private readonly IGenesis _genesis;
        private readonly IGameWorldConfig _config;

        public GameWorldFeature(IGenesis genesis, IGameWorldConfig config)
        {
            _genesis = genesis;
            _config = config;
        }
        
        protected override void Initialize()
        {
            RegisterRequest<PauseRequest>();
            RegisterRequest<ClickRequest>();
            RegisterRequest<InitializeGameWorldRequest>();
            RegisterRequest<ResetGameWorldRequest>();
            
            AddInitializer(new InitializeTicksSystem(_config));
            
            AddSystem(new ResetGameWorldSystem());
            AddSystem(new TransformClickRequestSystem());
            AddSystem(new PauseSystem());
            AddSystem(new TickSystem(_config));
            AddSystem(new InitializeGameWorldSystem(_genesis, _config));
            
            RegisterEvent<WorldInitializedEvent>();
            RegisterEvent<TickEvent>();
            RegisterEvent<PixelClickedEvent>();
            RegisterEvent<WorldResetEvent>();
        }
    }

    public class GameWorldLateFeature : LateUpdateFeature
    {
        private readonly IGameWorldConfig _config;

        public GameWorldLateFeature(IGameWorldConfig config)
        {
            _config = config;
        }
        
        protected override void Initialize()
        {
            AddSystem(new DrawGameWorldSystem(_config));
            AddSystem(new InitializeWorldAfterResetSystem());
        }
    }
}