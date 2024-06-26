using Genesis.Common.Components;

namespace Genesis.GameWorld
{
    public interface IGenesis
    {
        public IPixel CreatePixel(int width, int height, int x, int y);
        public void PostProcess(ref WorldComponent cWorld);
    }
}