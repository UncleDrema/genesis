using System;
using System.Collections.Generic;
using UnityEngine;
using Genesis.GameWorld;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Geneses.ArtLife
{
    public class ArtLifeCell
    {
        private ArtLifeWorld _world;
        public CellList.CellNode Node;
        
        // Параметры клетки
        public byte[] Genome { get; private set; } = new byte[256];
        public int GeneCounter { get; private set; } // индекс текущей активной ячейки генома
        public int Rotation { get; private set; } // направление от 0 (вправо) до 7 (право-вниз)
        public ArtLifePixel Position { get; set; } // позиция в мире
        public int Energy { get; private set; }
        public int Age { get; private set; }
        
        public int TotalPhotosynthesisEnergy { get; private set; } // Всего энергии, полученной от фотосинтеза
        public int TotalMineralCount { get; private set; } // Всего поглощенных минералов
        public int TotalOrganicCount { get; private set; } // Всего поглощенной органики
        public int AccumulatedMineralsCount { get; private set; }

        public ArtLifeCell(ArtLifeWorld world, ArtLifePixel position)
        {
            Assert.IsNull(position.Cell);
            _world = world;
            for (int i = 0; i < Genome.Length; i++)
            {
                Genome[i] = 0;
            }
            GeneCounter = 0;
            Rotation = 0;
            Position = position;
            position.SetCell(this);
            Energy = 50;
            Age = 0;
        }
        
        public void FillGenomeWithRandomValues()
        {
            for (int i = 0; i < Genome.Length; i++)
            {
                Genome[i] = (byte)Random.Range(0, 256);
            }
        }
        
        public void FillGenomeWithValue(byte value)
        {
            for (int i = 0; i < Genome.Length; i++)
            {
                Genome[i] = value;
            }
        }
        
        public void CopyGenomeFrom(ArtLifeCell cell)
        {
            for (int i = 0; i < Genome.Length; i++)
            {
                Genome[i] = cell.Genome[i];
            }
        }
        
        public void Tick()
        {
            if (Energy <= 0)
            {
                Die_Naturally();
                return;
            }
            
            if (Age >= _world.Config.MaxAge)
            {
                Die_Naturally();
                return;
            }

            if (Energy > _world.Config.EnergyToDuplicate)
            {
                Duplicate_EnoughEnergy();
                return;
            }

            /*
            if (Energy > _world.Config.OverloadEnergyCount)
            {
                Position.MineralCount = Math.Min(_world.Config.MineralMaxCount,
                    Position.MineralCount + _world.Config.DeathOrganicSpawnCount);
                Die_Naturally();
            }
            */

            AccumulateMinerals();
            
            ExecuteCommand();
            Age++;
            Energy -= _world.Config.EnergySpendPerTick;
            Position.IsDirty = true;
        }

        private void AccumulateMinerals()
        {
            AccumulatedMineralsCount += Position.MineralCount;
            AccumulatedMineralsCount = Math.Min(AccumulatedMineralsCount, _world.Config.MineralMaxCount);
        }

        public void MoveToPosition_IfEmpty(ArtLifePixel newPosition)
        {
            if (newPosition.IsEmpty)
            {
                Position.MakeEmpty();
                Position = newPosition;
                newPosition.SetCell(this);
            }
        }

        private void Mutate()
        {
            int mutationIndex = Random.Range(0, Genome.Length);
            Genome[mutationIndex] = (byte)Random.Range(0, 256);
        }

        private void Die_Naturally()
        {
            Position.OrganicCount = Math.Min(_world.Config.OrganicMaxCount,
                Position.OrganicCount + _world.Config.DeathOrganicSpawnCount);
            _world.RemoveCell(this);
        }
        
        private ArtLifePixel FindFirstNeighbour(int startDirection, Predicate<ArtLifePixel> predicate)
        {
            for (int i = 0; i < 8; i++)
            {
                var neighbor = Position.Neighbors[(Rotation + i) % 8];
                if (predicate(neighbor))
                {
                    return neighbor;
                }
            }
            return null;
        }

        // Поиск среды подходящих под критерий клетки с наибольшейшим значением метрики
        private ArtLifePixel FindBestNeighbour(int startDirection, Predicate<ArtLifePixel> predicate,
            Func<ArtLifePixel, float> metric, bool includeSelf = false)
        {
            ArtLifePixel bestPixel = null;
            float bestValue = float.MinValue;
            for (int i = 0; i < 8; i++)
            {
                var neighbor = Position.Neighbors[(Rotation + i) % 8];
                if (predicate(neighbor))
                {
                    var value = metric(neighbor);
                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestPixel = neighbor;
                    }
                }
            }
            
            if (includeSelf && predicate(Position))
            {
                var value = metric(Position);
                if (value > bestValue)
                {
                    bestValue = value;
                    bestPixel = Position;
                }
            }
            
            return bestPixel;
        }
        
        private void Duplicate_EnoughEnergy()
        {
            var freePosition = FindFirstNeighbour((Rotation + GeneCounter) % 8,
                p => p.IsEmpty && p.OrganicCount < _world.Config.OrganicMaxCount);

            if (freePosition != null)
            {
                var newCell = _world.CreateCell(freePosition);
                newCell.CopyGenomeFrom(this);
                if (Random.value < _world.Config.MutationChance)
                {
                    newCell.Mutate();
                }
                newCell.Rotation = Rotation;
                newCell.Energy = Energy / 3;
                Energy = Energy / 3;
                newCell.AccumulatedMineralsCount = AccumulatedMineralsCount / 2;
                AccumulatedMineralsCount = AccumulatedMineralsCount / 2;
            }
            else
            {
                Die_Naturally();
            }
        }

        public Color GetColor()
        {
            var totalConsumedEnergy = TotalPhotosynthesisEnergy + TotalMineralCount + TotalOrganicCount;
            
            var redFactor = Mathf.Clamp01((float)TotalOrganicCount / totalConsumedEnergy);
            var greenFactor = Mathf.Clamp01((float)TotalPhotosynthesisEnergy / totalConsumedEnergy);
            var blueFactor = Mathf.Clamp01((float)TotalMineralCount / totalConsumedEnergy);
            
            var color = new Color(redFactor, greenFactor, blueFactor);
            return color;
        }

        private void ExecuteCommand()
        {
            int command = Genome[GeneCounter];
            switch (command)
            {
                case 0: Photosynthesis(); break;
                case 1: Move(); break;
                case 2: ConsumeOrganics(); break;
                case 3: ConvertMineralsToEnergy(); break;
                case 4: Rotate(); break;
                default:
                    GeneCounter += command;
                    break;
            }
            GeneCounter = GeneCounter % Genome.Length;
        }

        private byte GetGeneArgument(int index)
        {
            return Genome[(GeneCounter + index) % Genome.Length];
        }
        
        private void Photosynthesis()
        {
            if (Position.PhotosynthesisEnergy > 0)
            {
                var mineralsMult = 1 + _world.Config.MineralsPhotosynthesisMultiplier * AccumulatedMineralsCount / _world.Config.MineralMaxCount;
                var photosynthesisEnergy = Mathf.RoundToInt(Position.PhotosynthesisEnergy * mineralsMult);
                Energy += photosynthesisEnergy;
                TotalPhotosynthesisEnergy += photosynthesisEnergy;
            }
            GeneCounter += 1;
        }
        
        private void Move()
        {
            var searchDirection = (Rotation + GetGeneArgument(1)) % 8;
            var newPosition = Position.Neighbors[searchDirection];
            MoveToPosition_IfEmpty(newPosition);
            GeneCounter += 2;
        }
        
        private void ConsumeOrganics()
        {
            // Ищем пустую клетку с самым большим количеством органики рядом
            var bestPosition = FindBestNeighbour((Rotation + GeneCounter) % 8,
                p => p.IsEmpty && p.OrganicCount > 0,
                p => p.OrganicCount,
                includeSelf: true);
            
            if (bestPosition != null)
            {
                // Потребляем органику
                var consumedOrganics = bestPosition.OrganicCount;
                bestPosition.OrganicCount = 0;
                Energy += consumedOrganics;
                TotalOrganicCount += consumedOrganics;
            }
            
            GeneCounter += 1;
        }
        
        private void ConvertMineralsToEnergy()
        {
            if (Position.MineralCount > 0)
            {
                Energy += AccumulatedMineralsCount;
                TotalMineralCount += AccumulatedMineralsCount;
                AccumulatedMineralsCount = 0;
            }
            GeneCounter += 1;
        }
        
        private void Rotate()
        {
            var newRotation = GetGeneArgument(1) % 8;
            Rotation = newRotation;
            GeneCounter += 2;
        }
    }
}