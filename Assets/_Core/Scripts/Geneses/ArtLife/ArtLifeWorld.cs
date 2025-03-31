using System.Collections.Generic;

namespace Geneses.ArtLife
{
    public class ArtLifeWorld
    {
        private List<ArtLifePixel> _pixels = new List<ArtLifePixel>();

        public void AddPixel(ArtLifePixel pixel)
        {
            _pixels.Add(pixel);
        }

        public void Tick()
        {
            foreach (var pixel in _pixels)
            {
                pixel.Tick();
            }
        }
    }
}