using System.Collections.Generic;
using UnityEngine;
using Genesis.GameWorld;
using UnityEngine.Assertions;

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
            Energy = 100;
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
                Die_NotEnoughEnergy();
                return;
            }
            
            if (Age >= 100)
            {
                Die_TooOld();
                return;
            }

            if (Energy > 100)
            {
                Duplicate_EnoughEnergy();
                return;
            }
            
            ExecuteCommand();
            Age++;
            Energy--;
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
        
        private void Die_TooOld()
        {
            Position.OrganicCount += 10;
            _world.RemoveCell(this);
        }

        private void Die_NotEnoughEnergy()
        {
            Position.OrganicCount += 10;
            _world.RemoveCell(this);
        }
        
        private ArtLifePixel FirstEmptyStartingFrom(int direction)
        {
            for (int i = 0; i < 8; i++)
            {
                var neighbor = Position.Neighbors[(direction + i) % 8];
                if (neighbor.IsEmpty)
                {
                    return neighbor;
                }
            }
            
            return null;
        }
        
        private void Duplicate_EnoughEnergy()
        {
            var freePosition = FirstEmptyStartingFrom(Rotation);
            if (freePosition != null)
            {
                var newCell = _world.CreateCell(freePosition);
                newCell.CopyGenomeFrom(this);
                newCell.Rotation = Rotation;
                newCell.Energy = Energy / 2;
                Energy = Energy / 2;
            }
        }

        public Color GetColor()
        {
            return Color.black;
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
                Position.OrganicCount = 0;
            }
            GeneCounter += 1;
        }
        
        private void ConsumeMinerals()
        {
            if (Position.MineralCount > 0)
            {
                Energy += Position.MineralCount;
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