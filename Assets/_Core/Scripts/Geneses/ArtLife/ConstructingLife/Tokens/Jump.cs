namespace Geneses.ArtLife.ConstructingLife.Tokens
{
    public class Jump : LifeToken
    {
        public byte Value { get; }
        
        public Jump(byte value)
        {
            Value = value;
        }
    }
}