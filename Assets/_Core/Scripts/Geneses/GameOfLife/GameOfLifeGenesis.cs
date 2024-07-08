using Genesis.Common.Components;
using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.GameOfLife
{
    public class GameOfLifeGenesis : IGenesis
    {
        public IPixel CreatePixel(int width, int height, int x, int y)
        {
            return CreateRandomPixels(width, height, x, y);
        }

        private IPixel CreateBar(int width, int height, int x, int y)
        {
            if (x == 64)
            {
                if (y is 40 or 41 or 42)
                {
                    return new GameOfLifePixel() { State = 1 };
                }
            }

            return new GameOfLifePixel() { State = 0 };
        }

        private IPixel CreateRandomPixels(int width, int height, int x, int y)
        {
            return new GameOfLifePixel()
            {
                State = Random.value > 0.5f ? 1 : 0
            };
        }

        public void PostProcess(ref WorldComponent cWorld)
        {
            
        }
    }
}