using System.Collections.Generic;
using UnityEngine;

namespace Hexa.Domain.Board
{
    public class HexGrid : IHexGrid
    {
        private readonly Dictionary<Vector2Int, IHexCell> _cells;

        public HexGrid(Dictionary<Vector2Int, IHexCell> cells)
        {
            _cells = cells;
        }

        public IReadOnlyDictionary<Vector2Int, IHexCell> Cells => _cells;

        public bool TryGetCell(Vector2Int coordinates, out IHexCell cell)
        {
            return _cells.TryGetValue(coordinates, out cell);
        }

        public IHexCell GetCell(Vector2Int coordinates)
        {
            return _cells[coordinates];
        }

        public bool Contains(Vector2Int coordinates)
        {
            return _cells.ContainsKey(coordinates);
        }
    }
}
