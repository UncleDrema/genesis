namespace Geneses.ArtLife.ConstructingLife.Tokens
{
    public class GenomeOperationArgument : LifeToken
    {
        public byte Value { get; }
            
        public GenomeOperationArgument(byte value)
        {
            Value = value;
        }
    }
}