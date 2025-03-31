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
    }
}
