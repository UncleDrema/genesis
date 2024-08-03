namespace Genesis.GameWorld
{
    public interface IGameWorldConfig
    {
        int WorldWidth { get; }
        int WorldHeight { get; }
        int PixelSize { get; }
        float TickPeriod { get; }
    }
}