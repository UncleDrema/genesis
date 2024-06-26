using Genesis.GameWorld.Events;
using Genesis.GameWorld.Requests;
using Genesis.GameWorld.Systems;
using Scellecs.Morpeh.Addons.Feature;

namespace Genesis.GameWorld
{
    public class GameWorldFeature : UpdateFeature
    {
        private readonly IGenesis _genesis;

        public GameWorldFeature(IGenesis genesis)
        {
            _genesis = genesis;
        }
        
        protected override void Initialize()
        {
            RegisterRequest<InitializeGameWorldRequest>();
            
            AddInitializer(new InitializeTicksSystem());
            
            AddSystem(new TickSystem());
            AddSystem(new InitializeGameWorldSystem(_genesis)); ;
            
            RegisterEvent<WorldInitializedEvent>();
            RegisterEvent<TickEvent>();
        }
    }

    public class GameWorldLateFeature : LateUpdateFeature
    {
        protected override void Initialize()
        {
            AddSystem(new DrawGameWorldSystem());
        }
    }
}