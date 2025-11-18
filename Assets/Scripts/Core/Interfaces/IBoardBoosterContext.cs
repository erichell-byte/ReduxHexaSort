using Gameplay.Towers;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IBoardBoosterContext
    {
        bool TryDestroyTower(BoardTower targetTower);
        bool TryDestroyTower(Vector2Int gridPosition);
        bool TryUndoLastAction();
        bool TryShuffleBoard();
        bool CanUndo { get; }
    }
}
