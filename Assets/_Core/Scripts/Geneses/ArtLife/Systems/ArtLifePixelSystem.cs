using Geneses.ArtLife.Components;
using Scellecs.Morpeh;
using UnityEngine;
namespace Geneses.ArtLife.Systems
{

    public sealed class ArtLifePixelSystem : ISystem {
        public World World { get; set; }
        
        private Filter _pixelFilter;
        private Filter _mineralFilter;
        private Filter _organicFilter;
        private Stash<ArtLifePixelComponent> _pixelStash;

        public void OnAwake() {
            _pixelFilter = World.Filter.With<ArtLifePixelComponent>().Build();
            _mineralFilter = World.Filter.With<MineralComponent>().Build();
            _organicFilter = World.Filter.With<OrganicComponent>().Build();
            _pixelStash = World.GetStash<ArtLifePixelComponent>();
        }
        public void OnUpdate(float deltaTime) {
            foreach (var entity in _pixelFilter) {
                ref var pixel = ref _pixelStash.Get(entity);
                if (pixel.Energy <= 0) continue;
            
                pixel.Age++;
                ExecuteCommand(ref pixel);
            }
        }

        private void ExecuteCommand(ref ArtLifePixelComponent pixel) {
            int command = pixel.Genome[pixel.ActiveGeneIndex];
            switch (command % 12) {
                case 0:
                    Move(ref pixel);
                    break;
                case 1:
                    IncreaseEnergy(ref pixel);
                    break;
                case 2:
                    Photosynthesis(ref pixel);
                    break;
                case 3:
                    ConvertMinerals(ref pixel);
                    break;
                case 4:
                    Eat(ref pixel);
                    break;
                case 5:
                    Look(ref pixel);
                    break;
                case 6:
                    Turn(ref pixel);
                    break;
                case 7:
                    ShareResources(ref pixel);
                    break;
                case 8:
                    CheckEnergy(ref pixel);
                    break;
                case 9:
                    CheckMinerals(ref pixel);
                    break;
                case 10:
                    CheckSurroundings(ref pixel);
                    break;
                case 11:
                    CheckOrganic(ref pixel);
                    break;
            }
            pixel.ActiveGeneIndex = (pixel.ActiveGeneIndex + pixel.CommandCounter) % pixel.Genome.Length;
        }
        private void Move(ref ArtLifePixelComponent pixel) {
            int directionGene = pixel.Genome[(pixel.ActiveGeneIndex + 1) % pixel.Genome.Length];
            pixel.Direction = directionGene % 8;
            
            Vector2 newPos = pixel.Position + GetDirectionVector(pixel.Direction);
            if (IsPositionEmpty(newPos)) {
                pixel.Position = newPos;
            }
            pixel.CommandCounter += 1;
        }

        private void IncreaseEnergy(ref ArtLifePixelComponent pixel) {
            int prevGeneIndex = (pixel.ActiveGeneIndex - 1 + pixel.Genome.Length) % pixel.Genome.Length;
            pixel.Energy += pixel.Genome[prevGeneIndex];
            pixel.CommandCounter += 3;
        }

        private void Photosynthesis(ref ArtLifePixelComponent pixel) {
            pixel.Energy += 15;
            pixel.CommandCounter += 5;
        }

        private void ConvertMinerals(ref ArtLifePixelComponent pixel) {
            foreach (var entity in _mineralFilter) {
                var mineral = entity.GetComponent<MineralComponent>();
                if (mineral.Position == pixel.Position) {
                    pixel.Energy += mineral.Value;
                    entity.RemoveComponent<MineralComponent>();
                    break;
                }
            }
            pixel.CommandCounter += 1;
        }

        private void Eat(ref ArtLifePixelComponent pixel) {
            foreach (var entity in _organicFilter) {
                var organic = entity.GetComponent<OrganicComponent>();
                if (organic.Position == pixel.Position) {
                    pixel.Energy += 20;
                    entity.RemoveComponent<OrganicComponent>();
                    break;
                }
            }
            pixel.CommandCounter += 4;
        }

        private void Look(ref ArtLifePixelComponent pixel) {
            int lookDirection = pixel.Genome[(pixel.ActiveGeneIndex + 1) % pixel.Genome.Length] % 8;
            Vector2 lookPos = pixel.Position + GetDirectionVector(lookDirection);
            
            pixel.CommandCounter += GetCellCodeAtPosition(lookPos);
            pixel.CommandCounter += 2;
        }

        private void Turn(ref ArtLifePixelComponent pixel) {
            int turnValue = pixel.Genome[(pixel.ActiveGeneIndex + 1) % pixel.Genome.Length];
            pixel.Direction = (pixel.Direction + turnValue) % 8;
            pixel.CommandCounter += 1;
        }

        private void ShareResources(ref ArtLifePixelComponent pixel) {
            if (pixel.Energy > 0) {
                int shareAmount = pixel.Energy / 2;
                Vector2 targetPos = pixel.Position + GetDirectionVector(pixel.Direction);
                
                foreach (var entity in _pixelFilter) {
                    var targetPixel = entity.GetComponent<ArtLifePixelComponent>();
                    if (targetPixel.Position == targetPos) {
                        targetPixel.Energy += shareAmount;
                        pixel.Energy -= shareAmount;
                        break;
                    }
                }
            }
            pixel.CommandCounter += 3;
        }

        private void CheckEnergy(ref ArtLifePixelComponent pixel) {
            if (pixel.Energy > 50) {
                pixel.ActiveGeneIndex = 16 % pixel.Genome.Length;
            }
            pixel.CommandCounter += 2;
        }

        private void CheckMinerals(ref ArtLifePixelComponent pixel) {
            foreach (var entity in _mineralFilter) {
                var mineral = entity.GetComponent<MineralComponent>();
                if (IsNeighbor(pixel.Position, mineral.Position)) {
                    pixel.ActiveGeneIndex = 24 % pixel.Genome.Length;
                    break;
                }
            }
            pixel.CommandCounter += 2;
        }

        private void CheckSurroundings(ref ArtLifePixelComponent pixel) {
            bool surrounded = true;
            for (int i = 0; i < 8; i++) {
                Vector2 checkPos = pixel.Position + GetDirectionVector(i);
                if (IsPositionEmpty(checkPos)) {
                    surrounded = false;
                    break;
                }
            }
            if (surrounded) {
                pixel.ActiveGeneIndex = 32 % pixel.Genome.Length;
            }
            pixel.CommandCounter += 2;
        }

        private void CheckOrganic(ref ArtLifePixelComponent pixel) {
            foreach (var entity in _organicFilter) {
                var organic = entity.GetComponent<OrganicComponent>();
                if (IsNeighbor(pixel.Position, organic.Position)) {
                    pixel.ActiveGeneIndex = 48 % pixel.Genome.Length;
                    break;
                }
            }
            pixel.CommandCounter += 2;
        }
        
        private Vector2 GetDirectionVector(int direction) {
            float angle = direction * 45 * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private bool IsPositionEmpty(Vector2 position) {
            foreach (var entity in _pixelFilter) {
                var pixel = entity.GetComponent<ArtLifePixelComponent>();
                if (pixel.Position == position) return false;
            }
            return true;
        }

        private int GetCellCodeAtPosition(Vector2 position) {
            foreach (var entity in _pixelFilter) {
                var pixel = entity.GetComponent<ArtLifePixelComponent>();
                if (pixel.Position == position) return pixel.Genome[0];
            }
            return 0;
        }

        private bool IsNeighbor(Vector2 a, Vector2 b) {
            return Vector2.Distance(a, b) <= 1.5f;
        }

        
        public void Dispose()
        {
        }
    }
}