namespace Geneses.ArtLife
{
    public interface IArtLifeConfig
    {
        float MutationChance { get; }
        float DuplicateMutationChance { get; }
        int MaxEnergy { get; }
        int MaxAge { get; }
        int PhotosynthesisEnergyMax { get; }
        float PhotosynthesisLevel { get; }
        float MineralsLevel { get; }
        int MineralLayersCount { get; }
        int MineralsPerLayer { get; }
        int MaxAccumulatedMinerals { get; }
        int MineralsToEnergy { get; }
        float MineralsPhotosynthesisMultiplier { get; }
        int EnergySpendPerTick { get; }
        int EnergyFromOrganic { get; }
        int MaxMineralsToConvert { get; }
        float RadiationRadius { get; }
        float RadiationChance { get; }
        public DrawMode DrawMode { get; }
    }
}