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
            public int MaxEnergy { get; } = 1000;
            public int MaxAge { get; } = 100;
            public int PhotosynthesisEnergyMax { get; } = 10;
            public float PhotosynthesisLevel { get; } = 0.85f;
            public float MineralsLevel { get; } = 0.5f;
            public int MineralLayersCount { get; } = 5;
            public int MineralsPerLayer { get; } = 5;
            public int MaxAccumulatedMinerals { get; } = 1000;
            public int MineralsToEnergy { get; } = 8;
            public float MineralsPhotosynthesisMultiplier { get; } = 1f;
            public int EnergySpendPerTick { get; } = 3;
            public int EnergyFromOrganic { get; } = 100;
            public int MaxMineralsToConvert { get; } = 100;
        }
    }
}