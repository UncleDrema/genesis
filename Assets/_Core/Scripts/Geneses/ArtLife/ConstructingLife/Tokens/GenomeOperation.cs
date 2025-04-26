namespace Geneses.ArtLife.ConstructingLife.Tokens
{
    public class GenomeOperation : LifeToken
    {
        public ArtLifeGenome Genome { get; }
            
        public GenomeOperation(ArtLifeGenome genome)
        {
            Genome = genome;
        }
    }
}