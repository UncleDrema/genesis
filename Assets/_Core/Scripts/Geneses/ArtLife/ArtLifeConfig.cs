using TriInspector;
using UnityEngine;

namespace Geneses.ArtLife
{
    [CreateAssetMenu(fileName = nameof(ArtLifeConfig), menuName = "Game/" + nameof(ArtLifeConfig))]
    public class ArtLifeConfig : ScriptableObject, IArtLifeConfig
    {
        [field: SerializeField, Range(0f, 0.1f)]
        public float MutationChance { get; set; } = 0.001f;
        [field: SerializeField, Range(0f, 1f)]
        public float DuplicateMutationChance { get; set; } = 0.25f;
        [field: SerializeField, Range(250, 2500)]
        public int MaxEnergy { get; set; } = 1000;
        [field: SerializeField, Range(50, 200)]
        public int MaxAge { get; set; } = 100;
        [field: SerializeField, Range(1, 20)]
        public int PhotosynthesisEnergyMax { get; set; } = 10;
        [field: SerializeField, Range(0f, 1f)]
        public float PhotosynthesisLevel { get; set; } = 0.85f;
        [field: SerializeField, Range(0f, 1f)]
        public float MineralsLevel { get; set; } = 0.5f;
        [field: SerializeField, Range(1, 10)]
        public int MineralLayersCount { get; set; } = 5;
        [field: SerializeField, Range(1, 10)]
        public int MineralsPerLayer { get; set; } = 5;
        [field: SerializeField, Range(250, 2500)]
        public int MaxAccumulatedMinerals { get; set; } = 1000;
        [field: SerializeField, Range(1, 10)]
        public int MineralsToEnergy { get; set; } = 2;
        [field: SerializeField, Range(0f, 2f)]
        public float MineralsPhotosynthesisMultiplier { get; set; } = 1f;
        [field: SerializeField, Range(1, 10)]
        public int EnergySpendPerTick { get; set; } = 3;
        [field: SerializeField, Range(1, 10)]
        public int EnergyFromOrganic { get; set; } = 3;
        [field: SerializeField, Range(10, 500)]
        public int MaxMineralsToConvert { get; set; } = 100;
        [field: SerializeField, Range(0, 100)]
        public float RadiationRadius { get; set; } = 50;
        [field: SerializeField, Range(0, 1f)]
        public float RadiationChance { get; set; } = 0.1f;
        [field: SerializeField]
        public ColorMap EnergyColorMap { get; set; }
        [field: SerializeField]
        public ColorMap AgeColorMap { get; set; }
        [field: SerializeField]
        public ColorMap MutationsColorMap { get; set; }
        [field: SerializeField]
        public ColorMap MineralsColorMap { get; set; }
        
        [Button]
        public void ResetValues()
        {
            var instance = new ArtLifeConfig();
            MutationChance = instance.MutationChance;
            DuplicateMutationChance = instance.DuplicateMutationChance;
            MaxEnergy = instance.MaxEnergy;
            MaxAge = instance.MaxAge;
            PhotosynthesisEnergyMax = instance.PhotosynthesisEnergyMax;
            PhotosynthesisLevel = instance.PhotosynthesisLevel;
            MineralsLevel = instance.MineralsLevel;
            MineralLayersCount = instance.MineralLayersCount;
            MineralsPerLayer = instance.MineralsPerLayer;
            MaxAccumulatedMinerals = instance.MaxAccumulatedMinerals;
            MineralsToEnergy = instance.MineralsToEnergy;
            MineralsPhotosynthesisMultiplier = instance.MineralsPhotosynthesisMultiplier;
            EnergySpendPerTick = instance.EnergySpendPerTick;
            EnergyFromOrganic = instance.EnergyFromOrganic;
            MaxMineralsToConvert = instance.MaxMineralsToConvert;
            RadiationRadius = instance.RadiationRadius;
            RadiationChance = instance.RadiationChance;
        }
    }
}