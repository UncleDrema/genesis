namespace Geneses.ArtLife.ConstructingLife
{
    public enum AbsoluteDirection : byte
    {
        Right = 0,
        DownRight = 1,
        Down = 2,
        DownLeft = 3,
        Left = 4,
        UpLeft = 5,
        Up = 6,
        UpRight = 7
    }
    
    public enum RelativeDirection : byte
    {
        Forward = 0,
        ForwardRight = 1,
        Right = 2,
        BackwardRight = 3,
        Backward = 4,
        BackwardLeft = 5,
        Left = 6,
        ForwardLeft = 7
    }
    
    public static class DirectionConversions
    {
        public static byte Value(this AbsoluteDirection direction)
        {
            return (byte)direction;
        }
        
        public static byte Value(this RelativeDirection direction)
        {
            return (byte)direction;
        }
    }
}