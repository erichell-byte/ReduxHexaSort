using System.Collections.Generic;
using Gameplay.Towers;
using Hexa.Domain.Board;
using UnityEngine;

namespace Core.Interfaces
{
    public interface ITowerPlacementService
    {
        void Initialize(IHexGrid grid, Dictionary<Vector2Int, BoardTower> boardTowers);
        bool TryPlaceTower(Vector3 dropPosition, IHexCell playerTower, out Vector2Int? placedPosition);
        BoardTower FindClosestEmptyCell(Vector3 position);
        Vector2Int? FindCellPositionByTower(BoardTower tower);
    }
}
