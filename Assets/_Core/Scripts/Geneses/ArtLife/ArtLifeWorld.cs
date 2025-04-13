using System.Collections.Generic;

namespace Geneses.ArtLife
{
    public class ArtLifeWorld
    {
        private readonly CellList _cellList = new();

        public ArtLifeCell CreateCell(ArtLifePixel pixel)
        {
            var cell = new ArtLifeCell(this, pixel);
            _cellList.Add(cell);
            pixel.SetCell(cell);
            return cell;
        }
        
        public void RemoveCell(ArtLifeCell cell)
        {
            var node = cell.Node;
            _cellList.Remove(node);
            cell.Position.MakeEmpty();
            cell.Node = null;
            cell.Position = null;
        }

        public void Tick()
        {
            var zero = _cellList.Zero;
            var current = zero.Next;
            while (current != zero)
            {
                var cell = current.Cell;
                var next = current.Next;
                // Клетка может удалить себя из списка
                cell.Tick();
                current = next;
            }
        }
    }
}