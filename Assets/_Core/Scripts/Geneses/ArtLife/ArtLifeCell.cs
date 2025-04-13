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

            if (Energy > _world.Config.OverloadEnergyCount)
            {
                Position.MineralCount = Math.Min(_world.Config.MineralMaxCount,
                    Position.MineralCount + _world.Config.DeathOrganicSpawnCount);
                Die_Naturally();
            }
            
            ExecuteCommand();
            Age++;
            Energy -= _world.Config.EnergySpendPerTick;
            Position.IsDirty = true;
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
        
        private ArtLifePixel FirstEmptySuitableForDuplication(int direction)
        {
            for (int i = 0; i < 8; i++)
            {
                var neighbor = Position.Neighbors[(direction + i) % 8];
                // Подходят пустые клетки, в которых не слишком много минералов
                if (neighbor.IsEmpty && neighbor.MineralCount < _world.Config.MineralDuplicationLimit)
                {
                    return neighbor;
                }
            }
            
            return null;
        }
        
        private void Duplicate_EnoughEnergy()
        {
            var freePosition = FirstEmptySuitableForDuplication((Rotation + GeneCounter) % 8);
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
                case 3: ConsumeMinerals(); break;
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
                Energy += Position.PhotosynthesisEnergy;
                TotalPhotosynthesisEnergy += Position.PhotosynthesisEnergy;
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
            if (Position.OrganicCount > 0)
            {
                Energy += Position.OrganicCount;
                TotalOrganicCount += Position.OrganicCount;
                Position.OrganicCount = 0;
            }
            GeneCounter += 1;
        }
        
        private void ConsumeMinerals()
        {
            if (Position.MineralCount > 0)
            {
                Energy += Position.MineralCount;
                TotalMineralCount += Position.MineralCount;
                Position.MineralCount = 0;
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