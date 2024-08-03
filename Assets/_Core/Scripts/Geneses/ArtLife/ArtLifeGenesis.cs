using Genesis.Common.Components;
using Genesis.GameWorld;
using UnityEngine;

namespace Geneses.ArtLife
{
    public class ArtLifeGenesis : IGenesis
    {
        public IPixel CreatePixel(int width, int height, int x, int y)
        {
            return new ArtLifePixel();
        }

        public void PostProcess(ref WorldComponent cWorld)
        {
            
        }
    }
}