using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Gameplay.Board;
using Gameplay.Towers;
using Hexa.Domain.Board;
using Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ChainReactionService : IChainReactionService
    {
        public event Action<int> OnScoreAdded;
        
        private IHexGrid _grid;
        private Dictionary<Vector2Int, BoardTower> _boardTowers;
        private ITowerPlacementService _towerPlacement;
        private IAnimationBridge _animationBridge;
        private LevelConfig _levelConfig;

        
        private readonly float _pieceHeight = 0.2f;

        [Inject]
        public ChainReactionService(ITowerPlacementService towerPlacement, IAnimationBridge animationBridge, LevelConfig levelConfig)
        {
            _towerPlacement = towerPlacement;
            _animationBridge = animationBridge;
            _levelConfig = levelConfig;
        }

        public void Initialize(IHexGrid grid, Dictionary<Vector2Int, BoardTower> boardTowers)
        {
            _grid = grid;
            _boardTowers = boardTowers;
        }

        public async Task CheckChains(Vector2Int placedPosition)
        {
            if (_grid == null || _boardTowers == null)
            {
                return;
            }

            var positionsToProcess = new Queue<Vector2Int>();
            int iterations = 0;
            const int maxIterations = 100;

            positionsToProcess.Enqueue(placedPosition);

            while (positionsToProcess.Count > 0 && iterations < maxIterations)
            {
                iterations++;
                
                var currentPosition = positionsToProcess.Dequeue();
                
                if (!_boardTowers.TryGetValue(currentPosition, out var tower) || tower == null)
                    continue;

                await CheckTowerChains(currentPosition, tower, positionsToProcess);
                await CheckForMatchesInRadius(currentPosition, 0);
            }
        }

        private async Task CheckTowerChains(Vector2Int position, BoardTower tower, Queue<Vector2Int> positionsToProcess)
        {
            if (tower == null || tower.IsEmpty) return;

            var neighbors = GetNeighborTowers(position);
            var currentTopColor = tower.GetTopColor();

            if (!currentTopColor.HasValue) return;

            foreach (var neighbor in neighbors)
            {
                if (neighbor.tower == null || neighbor.tower.IsEmpty) continue;

                var neighborTopColor = neighbor.tower.GetTopColor();
                if (!neighborTopColor.HasValue) continue;

                if (currentTopColor.Value == neighborTopColor.Value)
                {
                    int currentCount = CountTopColorSequence(tower);
                    int neighborCount = CountTopColorSequence(neighbor.tower);

                    if (currentCount == 0 || neighborCount == 0) continue;

                    if (ShouldTransferToNeighbor(currentCount, neighborCount))
                    {
                        await TransferTopPiece(tower, neighbor.tower);
                    }
                    else
                    {
                        await TransferTopPiece(neighbor.tower, tower);
                    }

                    if (_boardTowers != null)
                    {
                        positionsToProcess.Enqueue(position);
                        positionsToProcess.Enqueue(neighbor.position);
                    }
                    return;
                }
            }
        }

        private async Task CheckForMatchesInRadius(Vector2Int center, int radius)
        {
            var positionsToCheck = GetTowersInRadius(center, radius);

            foreach (var position in positionsToCheck)
            {
                if (!_boardTowers.TryGetValue(position, out var tower) || tower == null)
                    continue;

                if (tower.IsEmpty) continue;

                var cell = _grid.GetCell(position);
                int sequenceCount = CountTopColorSequence(tower);
                
                if (sequenceCount >= _levelConfig.MatchCount)
                {
                    await RemoveTopSequence(cell, tower, sequenceCount);
                }
            }
        }

        private async Task TransferTopPiece(BoardTower fromTower, BoardTower toTower)
        {
            if (fromTower == null || toTower == null)
            {
                return;
            }

            var fromPosition = _towerPlacement.FindCellPositionByTower(fromTower);
            var toPosition = _towerPlacement.FindCellPositionByTower(toTower);

            if (!fromPosition.HasValue || !toPosition.HasValue) return;

            var fromCell = _grid.GetCell(fromPosition.Value);
            var toCell = _grid.GetCell(toPosition.Value);

            if (fromCell.IsEmpty) return;

            int fromTopColor = fromCell.GetTopColor().Value;
            int toTopColor = toCell.GetTopColor().Value;

            if (fromTopColor != toTopColor) return;

            int topColor = fromCell.RemoveTopColor();
            
            if (topColor != toTopColor)
            {
                fromCell.AddColor(topColor);
                return;
            }

            var topPiece = fromTower.GetTopPieceVisual();
            if (topPiece != null)
            {
                await _animationBridge.AnimatePieceTransfer(topPiece, 
                    toTower.transform.position + Vector3.up * (toCell.Stack.Count * _pieceHeight));
            }

            toCell.AddColor(topColor);

            fromTower?.RefreshVisual();
            toTower?.RefreshVisual();

            await Task.Delay(100);
        }

        private async Task RemoveTopSequence(IHexCell cell, BoardTower tower, int count)
        {
            if (tower == null)
            {
                return;
            }

            var piecesToRemove = new List<Transform>();
        
            for (int i = 0; i < count && !cell.IsEmpty; i++)
            {
                var topPiece = tower.GetTopPieceVisual();
                if (topPiece != null)
                {
                    piecesToRemove.Add(topPiece);
                    cell.RemoveTopColor();
                    
                    OnScoreAdded?.Invoke(1);
                }
            }
            
            foreach (var piece in piecesToRemove)
            {
                if (piece != null) 
                {
                    await _animationBridge.AnimatePieceRemoval(piece);
                }
            }
            
            tower.RefreshVisual();
        }


        private int CountTopColorSequence(BoardTower tower)
        {
            if (tower == null || tower.IsEmpty || _grid == null) return 0;

            var position = _towerPlacement.FindCellPositionByTower(tower);
            if (!position.HasValue) return 0;

            var cell = _grid.GetCell(position.Value);
            int targetColor = cell.GetTopColor().Value;
            int count = 0;

            var colors = cell.Stack.Colors;
            for (int i = colors.Count - 1; i >= 0; i--)
            {
                if (colors[i] == targetColor)
                    count++;
                else
                    break;
            }

            return count;
        }

        private bool ShouldTransferToNeighbor(int currentCount, int neighborCount)
        {
            return neighborCount > currentCount;
        }

        private List<(Vector2Int position, BoardTower tower)> GetNeighborTowers(Vector2Int position)
        {
            var neighbors = new List<(Vector2Int, BoardTower)>();
            if (_boardTowers == null)
            {
                return neighbors;
            }
            Vector2Int[] directions = {
                new(1, 0), new(1, -1), new(0, -1),
                new(-1, 0), new(-1, 1), new(0, 1)
            };

            foreach (var direction in directions)
            {
                var neighborPos = position + direction;
                if (_boardTowers.TryGetValue(neighborPos, out var tower))
                    neighbors.Add((neighborPos, tower));
            }

            return neighbors;
        }

        private HashSet<Vector2Int> GetTowersInRadius(Vector2Int center, int radius)
        {
            var positions = new HashSet<Vector2Int>();
            if (_boardTowers == null)
            {
                return positions;
            }

            for (int q = -radius; q <= radius; q++)
            {
                for (int r = -radius; r <= radius; r++)
                {
                    if (Math.Abs(q + r) > radius) continue;
                    var pos = center + new Vector2Int(q, r);
                    if (_boardTowers.ContainsKey(pos))
                        positions.Add(pos);
                }
            }

            return positions;
        }
    }
}
