namespace Geneses.ArtLife
{
    public class CellList
    {
        public CellNode Zero { get; private set; }

        public CellList()
        {
            Zero = new CellNode();
            Zero.Cell = null;
            Zero.Next = Zero;
            Zero.Prev = Zero;
        }
        
        public void Add(ArtLifeCell cell)
        {
            var node = new CellNode();
            cell.Node = node;
            node.Cell = cell;
            node.Next = Zero;
            node.Prev = Zero.Prev;
            Zero.Prev.Next = node;
            Zero.Prev = node;
        }

        public void Remove(CellNode node)
        {
            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;
            node.Next = null;
            node.Prev = null;
            node.Cell = null;
        }
        
        public class CellNode
        {
            public ArtLifeCell Cell { get; set; }
            public CellNode Next { get; set; }
            public CellNode Prev { get; set; }
        }
    }
}