using System.Collections.Generic;
using UnityEngine;

namespace Hexa.Domain.Board
{
    public interface IHexGrid
    {
        IReadOnlyDictionary<Vector2Int, IHexCell> Cells { get; }
        bool TryGetCell(Vector2Int coordinates, out IHexCell cell);
        IHexCell GetCell(Vector2Int coordinates);
        bool Contains(Vector2Int coordinates);
    }
}
