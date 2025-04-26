namespace Geneses.ArtLife.ConstructingLife.Tokens
{
    public class LabelPlaceholder : LifeToken
    {
        public string Name { get; }
            
        public int OffsetFromBranch { get; }
            
        public LabelPlaceholder(string name, int offsetFromBranch)
        {
            Name = name;
            OffsetFromBranch = offsetFromBranch;
        }
    }
}