namespace Geneses.ArtLife
{
    public interface IArtLifeConfig
    {
        float MutationChance { get; }
        int EnergyToDuplicate { get; }
        int MaxAge { get; }
        int PhotosynthesisEnergyMax { get; }
        float PhotosynthesisLevel { get; }
        float MineralsLevel { get; }
        int MineralLayersCount { get; }
        int MineralsPerLayer { get; }
        int DeathOrganicSpawnCount { get; }
        int MineralMaxCount { get; }
        int EnergySpendPerTick { get; }
        int OverloadEnergyCount { get; }
        int MineralDuplicationLimit { get; }
        int OrganicMaxCount { get; }
    }
}