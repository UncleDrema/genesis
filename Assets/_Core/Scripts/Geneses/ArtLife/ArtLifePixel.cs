using System.Collections.Generic;
using UnityEngine;
using Genesis.GameWorld;

namespace Geneses.ArtLife
{
    public class ArtLifePixel : IPixel
    {
        public bool IsDirty { get; set; }
        public Color32 Color => UnityEngine.Color.black;
        
        // Параметры клетки
        public byte[] Genome { get; private set; } = new byte[64];
        public int CommandCounter { get; private set; }
        public int ActiveGeneIndex { get; private set; } // индекс текущей активной ячейки генома
        public int Direction { get; private set; } // направление от 0 (вправо) до 7 (право-вниз)
        public Vector2 Position { get; set; } // позиция в мире
        public int Energy { get; private set; }
        public int Age { get; private set; }
        public int LastEnergySource { get; private set; }
        public HashSet<int> EnergySources { get; private set; } = new HashSet<int>();

        public ArtLifePixel()
        {
            for (int i = 0; i < Genome.Length; i++)
            {
                Genome[i] = (byte)Random.Range(0, 256);
            }
            CommandCounter = 0;
            ActiveGeneIndex = 0;
            Direction = 0;
            Position = Vector2Int.zero;
            Energy = 100; // начальное значение энергии
            Age = 0;
            LastEnergySource = 0;
        }
                public void Tick()
        {
            if (Energy <= 0) return;
            Age++;
            ExecuteCommand();
        }

        private void ExecuteCommand()
        {
            int command = Genome[ActiveGeneIndex];
            switch (command % 12)
            {
                case 0: Move(); break;
                case 1: IncreaseEnergy(); break;
                case 2: Photosynthesis(); break;
                case 3: ConvertMinerals(); break;
                case 4: Eat(); break;
                case 5: Look(); break;
                case 6: Turn(); break;
                case 7: ShareResources(); break;
                case 8: CheckEnergy(); break;
                case 9: CheckMinerals(); break;
                case 10: CheckSurroundings(); break;
                case 11: CheckOrganic(); break;
            }
            ActiveGeneIndex = (ActiveGeneIndex + CommandCounter) % Genome.Length;
        }

        private void Move() { /* Реализация передвижения */ CommandCounter += 1; }
        private void IncreaseEnergy() { /* Увеличение энергии */ CommandCounter += 3; }
        private void Photosynthesis() { /* Фотосинтез */ CommandCounter += 5; }
        private void ConvertMinerals() { /* Минералы -> энергия */ CommandCounter += 1; }
        private void Eat() { /* Поглощение органики */ CommandCounter += 4; }
        private void Look() { /* Осмотр окружающего мира */ CommandCounter += 2; }
        private void Turn() { /* Поворот */ CommandCounter += 1; }
        private void ShareResources() { /* Разделение ресурсов */ CommandCounter += 3; }
        private void CheckEnergy() { /* Проверка энергии */ CommandCounter += 2; }
        private void CheckMinerals() { /* Проверка минералов */ CommandCounter += 2; }
        private void CheckSurroundings() { /* Проверка окружения */ CommandCounter += 2; }
        private void CheckOrganic() { /* Проверка органики */ CommandCounter += 2; }
    }
}
