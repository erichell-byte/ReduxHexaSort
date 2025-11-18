using System.Collections.Generic;
using Core.Interfaces;
using Gameplay.Towers;
using Hexa.Domain.Board;
using UnityEngine;

namespace Services
{
    public class TowerPlacementService : ITowerPlacementService
    {
        private IHexGrid _grid;
        private Dictionary<Vector2Int, BoardTower> _boardTowers;

        public void Initialize(IHexGrid grid, Dictionary<Vector2Int, BoardTower> boardTowers)
        {
            _grid = grid;
            _boardTowers = boardTowers;
        }

        public bool TryPlaceTower(Vector3 dropPosition, IHexCell playerTower, out Vector2Int? placedPosition)
        {
            placedPosition = null;
            var targetTower = FindClosestEmptyCell(dropPosition);
            
            if (targetTower == null) return false;

            var cellPosition = FindCellPositionByTower(targetTower);
            if (!cellPosition.HasValue || !_grid.Contains(cellPosition.Value) || !_grid.GetCell(cellPosition.Value).IsEmpty)
            {
                return false;
            }

            var targetCell = _grid.GetCell(cellPosition.Value);
            
            foreach (var colorId in playerTower.Stack.Colors)
            {
                targetCell.AddColor(colorId);
            }
            playerTower.Clear();
            
            placedPosition = cellPosition.Value;
            return true;
        }

        public BoardTower FindClosestEmptyCell(Vector3 position)
        {
            BoardTower closestTower = null;
            float closestDistance = 1.5f;
            
            foreach (var tower in _boardTowers)
            {
                var boardTower = tower.Value;
                var cellPosition = tower.Key;

                if (boardTower == null)
                    continue;
        
                bool isEmpty = _grid.Contains(cellPosition) && _grid.GetCell(cellPosition).IsEmpty;
        
                if (isEmpty)
                {
                    float distance = Vector3.Distance(position, boardTower.transform.position);
            
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTower = boardTower;
                    }
                }
            }
    
            return closestTower;
        }

        public Vector2Int? FindCellPositionByTower(BoardTower tower)
        {
            foreach (var kvp in _boardTowers)
            {
                if (kvp.Value == tower)
                    return kvp.Key;
            }
            return null;
        }
    }
}
