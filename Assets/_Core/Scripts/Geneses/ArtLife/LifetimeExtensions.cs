using Genesis.GameWorld;
using VContainer;

namespace Geneses.ArtLife
{
    public static class LifetimeExtensions
    {
        public static IContainerBuilder RegisterArtLifeGenesis(this IContainerBuilder builder)
        {
            builder.RegisterInstance(new StubConfig()).As<IArtLifeConfig>();
            builder.Register<ArtLifeWorld>(Lifetime.Singleton);
            builder.Register<ArtLifeGenesis>(Lifetime.Singleton).As<IGenesis>();
            return builder;
        }

        private class StubConfig : IArtLifeConfig
        {
            public float MutationChance { get; } = 0.25f;
            public int EnergyToDuplicate { get; } = 100;
            public int MaxAge { get; } = 25;
            public int PhotosynthesisEnergyMax { get; } = 10;
            public float PhotosynthesisLevel { get; } = 0.85f;
            public float MineralsLevel { get; } = 0.5f;
            public int MineralLayersCount { get; } = 5;
            public int MineralsPerLayer { get; } = 4;
            public int DeathOrganicSpawnCount { get; } = 10;
            public int MineralMaxCount { get; } = 200;
            public float MineralsPhotosynthesisMultiplier { get; } = 2f;
            public int EnergySpendPerTick { get; } = 2;
            public int OrganicMaxCount { get; } = 100;
        }
    }
}