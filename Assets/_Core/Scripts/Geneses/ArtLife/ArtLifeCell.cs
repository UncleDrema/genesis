using System;
using System.Linq;
using Geneses.ArtLife.ConstructingLife;
using UnityEngine;
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
        public int TotalMineralEnergy { get; private set; } // Всего энергии, полученной от минералов
        public int TotalOrganicEnergy { get; private set; } // Всего энергии, полученной от органики
        public int AccumulatedMineralsCount { get; private set; } // Количество накопленных минералов
        public int ExecutedCommandsInCycle { get; private set; } // Количество выполненных команд в текущем цикле
        public bool BreakExecution { get; private set; } // Признак того, что клетка должна прервать выполнение команд в текущем цикле

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

        public void FillFromSource(byte[] source, byte remaining)
        {
            for (int i = 0; i < source.Length; i++)
            {
                Genome[i] = source[i];
            }
            for (int i = source.Length; i < Genome.Length; i++)
            {
                Genome[i] = remaining;
            }
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
            Position.IsDirty = true;
            Energy -= _world.Config.EnergySpendPerTick;
            
            if (Energy <= 0)
            {
                Die_Organic();
                return;
            }
            
            /*
            if (Age >= _world.Config.MaxAge)
            {
                Die_Organic();
                return;
            }
            */
            
            AccumulateMinerals();

            ExecutedCommandsInCycle = 0;
            BreakExecution = false;

            while (ExecutedCommandsInCycle < 15 && !BreakExecution)
            {
                ExecuteCommand();
                ExecutedCommandsInCycle++;
            }
            
            if (Energy > _world.Config.MaxEnergy)
            {
                Energy = _world.Config.MaxEnergy;
            }

            if (Energy == _world.Config.MaxEnergy)
            {
                Duplicate_MaxEnergy();
            }
            
            Age++;
        }

        private void AccumulateMinerals()
        {
            AccumulatedMineralsCount += Position.MineralCount;
            AccumulatedMineralsCount = Math.Min(AccumulatedMineralsCount, _world.Config.MaxAccumulatedMinerals);
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

        private void Die_Organic()
        {
            var position = Position;
            _world.RemoveCell(this);
            position.MakeOrganic();
        }
        
        private void Die_NoOrganic()
        {
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

        private void Duplicate_MaxEnergy()
        {
            Duplicate_Impl();
        }
        
        private void Duplicate_Impl()
        {
            var freePosition = FindFirstNeighbour((Rotation + Age) % 8, p => p.IsEmpty);

            if (freePosition != null)
            {
                var newCell = _world.CreateCell(freePosition);
                newCell.CopyGenomeFrom(this);
                if (Random.value < _world.Config.MutationChance)
                {
                    newCell.Mutate();
                }
                newCell.Rotation = Random.Range(0, 8);
                newCell.Energy = Energy / 2;
                Energy = Energy / 2;
                newCell.AccumulatedMineralsCount = AccumulatedMineralsCount / 2;
                AccumulatedMineralsCount = AccumulatedMineralsCount / 2;
            }
            else
            {
                Die_Organic();
            }
        }

        public Color GetColor()
        {
            var totalConsumedEnergy = TotalPhotosynthesisEnergy + TotalMineralEnergy + TotalOrganicEnergy;
            
            var redFactor = Mathf.Clamp01((float)TotalOrganicEnergy / totalConsumedEnergy);
            var greenFactor = Mathf.Clamp01((float)TotalPhotosynthesisEnergy / totalConsumedEnergy);
            var blueFactor = Mathf.Clamp01((float)TotalMineralEnergy / totalConsumedEnergy);
            
            var color = new Color(redFactor, greenFactor, blueFactor);
            return color;
        }

        private void GainPhotosynthesisEnergy(int energy)
        {
            TotalPhotosynthesisEnergy += energy;
            Energy += energy;
        }
        
        private void GainMineralEnergy(int energy)
        {
            TotalMineralEnergy += energy;
            Energy += energy;
        }
        
        private void GainOrganicEnergy(int energy)
        {
            TotalOrganicEnergy += energy;
            Energy += energy;
        }

        private void ExecuteCommand()
        {
            byte command = Genome[GeneCounter];
            /*
            Debug.Log($"Pointer at genome value: {command}");
            if (Enum.GetValues(typeof(LifeBuilder.ArtLifeGenome)).Cast<LifeBuilder.ArtLifeGenome>().Any(g => (byte)g == command))
            {
                Debug.Log($"\tCommand: {(LifeBuilder.ArtLifeGenome)command}");
            }
            */
            switch (command)
            {
                case 0: Photosynthesis(); StopExecuting(); break;
                case 1: Rotate(absolute: true); break;
                case 2: Rotate(absolute: false); break;
                case 3: Move(absolute: true); StopExecuting(); break;
                case 4: Move(absolute: false); StopExecuting(); break;
                case 5: Look(absolute: true); break;
                case 6: Look(absolute: false); break;
                case 7: AlignHorizontal(); break;
                case 8: AlignVertical(); break;
                case 9: Share(absolute: true); break;
                case 10: Share(absolute: false); break;
                case 11: Gift(absolute: true); break;
                case 12: Gift(absolute: false); break;
                case 13: Eat(absolute: true); StopExecuting(); break;
                case 14: Eat(absolute: false); StopExecuting(); break;
                case 15: ConvertMinerals(); StopExecuting(); break;
                case 16: Duplicate(); StopExecuting(); break;
                case 17: CheckEnergy(); break;
                case 18: CheckHeight(); break;
                case 19: CheckMinerals(); break;
                case 20: CheckSurrounded(); break;
                case 21: CheckPhotosynthesisFlow(); break;
                case 22: CheckMineralFlow(); break;
                case 255: GeneCounter = 0; break;
                
                default:
                    GeneCounter += command;
                    break;
            }

            GeneCounter %= Genome.Length;
        }

        private void StopExecuting()
        {
            BreakExecution = true;
        }
        
        private int GetDirection(bool absolute)
        {
            if (absolute)
            {
                return GetGeneArgument(1) % 8;
            }
            else
            {
                return (Rotation + GetGeneArgument(1)) % 8;
            }
        }

        private int GetArgumentNoFromPosition(ArtLifePixel pixel)
        {
            switch (pixel.Content)
            {
                case PixelContentType.Empty:
                    return 2;
                case PixelContentType.Wall:
                    return 3;
                case PixelContentType.Organic:
                    return 4;
                case PixelContentType.Cell:
                    return 5;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private byte GetGeneArgument(int index)
        {
            return Genome[(GeneCounter + index) % Genome.Length];
        }
        
        private void Photosynthesis()
        {
            if (Position.PhotosynthesisEnergy > 0)
            {
                var mineralsMult = 1 + _world.Config.MineralsPhotosynthesisMultiplier * AccumulatedMineralsCount / _world.Config.MaxAccumulatedMinerals;
                var photosynthesisEnergy = Mathf.RoundToInt(Position.PhotosynthesisEnergy * mineralsMult);
                GainPhotosynthesisEnergy(photosynthesisEnergy);
            }
            GeneCounter += 1;
        }
        
        private void Rotate(bool absolute)
        {
            Rotation = GetDirection(absolute);
            GeneCounter += 2;
        }

        private void Move(bool absolute)
        {
            var direction = GetDirection(absolute);
            var positionToMove = Position.Neighbors[direction];
            MoveToPosition_IfEmpty(positionToMove);
            GeneCounter += GetGeneArgument(GetArgumentNoFromPosition(positionToMove));
        }

        private void Look(bool absolute)
        {
            var direction = GetDirection(absolute);
            var positionToLook = Position.Neighbors[direction];
            GeneCounter += GetGeneArgument(GetArgumentNoFromPosition(positionToLook));
        }

        private void AlignHorizontal()
        {
            if (Random.value > 0.5f)
            {
                Rotation = (byte) LifeBuilder.Direction.Right;
            }
            else
            {
                Rotation = (byte) LifeBuilder.Direction.Left;
            }
            GeneCounter += 1;
        }
        
        private void AlignVertical()
        {
            if (Random.value > 0.5f)
            {
                Rotation = (byte) LifeBuilder.Direction.Up;
            }
            else
            {
                Rotation = (byte) LifeBuilder.Direction.Down;
            }
            GeneCounter += 1;
        }

        private void Share(bool absolute)
        {
            var direction = GetDirection(absolute);
            var position = Position.Neighbors[direction];
            if (position.Content is PixelContentType.Cell)
            {
                var other = position.Cell!;
                var totalEnergy = Energy + other.Energy;
                if (totalEnergy > 1)
                {
                    other.Energy = totalEnergy / 2;
                    Energy = totalEnergy / 2;
                }

                var totalMinerals = AccumulatedMineralsCount + other.AccumulatedMineralsCount;
                if (totalMinerals > 1)
                {
                    other.AccumulatedMineralsCount = totalMinerals / 2;
                    AccumulatedMineralsCount = totalMinerals / 2;
                }
            }
            GeneCounter += GetGeneArgument(GetArgumentNoFromPosition(position));
        }

        private void Gift(bool absolute)
        {
            var direction = GetDirection(absolute);
            var position = Position.Neighbors[direction];
            if (position.Content is PixelContentType.Cell)
            {
                var other = position.Cell!;
                if (other.Energy < Energy)
                {
                    var difference = Energy - other.Energy;
                    if (difference > 1)
                    {
                        other.Energy += difference / 2;
                        Energy -= difference / 2;
                    }
                }
                
                if (other.AccumulatedMineralsCount < AccumulatedMineralsCount)
                {
                    var difference = AccumulatedMineralsCount - other.AccumulatedMineralsCount;
                    if (difference > 1)
                    {
                        other.AccumulatedMineralsCount += difference / 2;
                        AccumulatedMineralsCount -= difference / 2;
                    }
                }
            }
            GeneCounter += GetGeneArgument(GetArgumentNoFromPosition(position));
        }

        private void Eat(bool absolute)
        {
            var direction = GetDirection(absolute);
            var positionToEat = Position.Neighbors[direction];
            
            // Съедаем органику
            if (positionToEat.Content is PixelContentType.Organic)
            {
                Energy += _world.Config.EnergyFromOrganic;
                positionToEat.MakeEmpty();
            }
            // Нападаем на клетку
            else if (positionToEat.Content is PixelContentType.Cell)
            {
                var other = positionToEat.Cell!;
                if (AccumulatedMineralsCount > other.AccumulatedMineralsCount)
                {
                    AccumulatedMineralsCount -= other.AccumulatedMineralsCount;
                    var gainedEnergy = _world.Config.EnergyFromOrganic + other.Energy / 2;
                    GainOrganicEnergy(gainedEnergy);
                    other.Die_NoOrganic();
                }
                else
                {
                    other.AccumulatedMineralsCount -= AccumulatedMineralsCount;
                    AccumulatedMineralsCount = 0;
                    // Если здоровья в 2 раза больше, чем у жертвы, пробиваем защиту
                    if (Energy >= other.AccumulatedMineralsCount * 2)
                    {
                        var gainedEnergy = _world.Config.EnergyFromOrganic + other.Energy / 2 - other.AccumulatedMineralsCount * 2;
                        GainOrganicEnergy(gainedEnergy);
                        other.Die_NoOrganic();
                    }
                    else
                    {
                        // Не хватило сил
                        Die_Organic();
                    }
                }
            }

            GeneCounter += GetGeneArgument(GetArgumentNoFromPosition(positionToEat));
        }


        private void ConvertMinerals()
        {
            var mineralsToConvert = Math.Min(AccumulatedMineralsCount, _world.Config.MaxMineralsToConvert);
            
            var energyFromMinerals = mineralsToConvert * _world.Config.MineralsToEnergy;
            GainMineralEnergy(energyFromMinerals);
            AccumulatedMineralsCount -= mineralsToConvert;
            
            GeneCounter += 1;
        }

        private void Duplicate()
        {
            Duplicate_Impl();
            
            GeneCounter += 1;
        }

        private void CheckEnergy()
        {
            var energyLevel = (float) Energy / _world.Config.MaxEnergy;
            var requiredEnergyLevel = GetGeneArgument(1) / 255f;
            if (energyLevel >= requiredEnergyLevel)
            {
                GeneCounter += GetGeneArgument(2);
            }
            else
            {
                GeneCounter += GetGeneArgument(3);
            }
        }

        private void CheckHeight()
        {
            var heightLevel = (float) Position.Y / _world.Height;
            var requiredHeightLevel = GetGeneArgument(1) / 255f;
            if (heightLevel >= requiredHeightLevel)
            {
                GeneCounter += GetGeneArgument(2);
            }
            else
            {
                GeneCounter += GetGeneArgument(3);
            }
        }

        private void CheckMinerals()
        {
            var mineralsLevel = (float) AccumulatedMineralsCount / _world.Config.MaxAccumulatedMinerals;
            var requiredMineralsLevel = GetGeneArgument(1) / 255f;
            if (mineralsLevel >= requiredMineralsLevel)
            {
                GeneCounter += GetGeneArgument(2);
            }
            else
            {
                GeneCounter += GetGeneArgument(3);
            }
        }

        private void CheckSurrounded()
        {
            var surrounded = true;
            for (int i = 0; i < 8; i++)
            {
                if (Position.Neighbors[i].IsEmpty)
                {
                    surrounded = false;
                    break;
                }
            }

            if (surrounded)
            {
                GeneCounter += GetGeneArgument(1);
            }
            else
            {
                GeneCounter += GetGeneArgument(2);
            }
        }

        private void CheckPhotosynthesisFlow()
        {
            var photosynthesisLevel = (float) Position.PhotosynthesisEnergy / _world.Config.PhotosynthesisEnergyMax;
            var requiredPhotosynthesisLevel = GetGeneArgument(1) / 255f;
            if (photosynthesisLevel >= requiredPhotosynthesisLevel)
            {
                GeneCounter += GetGeneArgument(2);
            }
            else
            {
                GeneCounter += GetGeneArgument(3);
            }
        }

        private void CheckMineralFlow()
        {
            var mineralsLevel = (float) Position.MineralCount / _world.Config.MineralsPerLayer * _world.Config.MineralsLevel;
            var requiredMineralsLevel = GetGeneArgument(1) / 255f;
            if (mineralsLevel >= requiredMineralsLevel)
            {
                GeneCounter += GetGeneArgument(2);
            }
            else
            {
                GeneCounter += GetGeneArgument(3);
            }
        }
    }
}