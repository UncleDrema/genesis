namespace Geneses.ArtLife.ConstructingLife.Tokens
{
    public class Label : LifeToken
    {
        public string Name { get; }
            
        public Label(string name)
        {
            Name = name;
        }
    }
}