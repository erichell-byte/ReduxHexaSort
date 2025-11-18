using Gameplay.Towers;
using UnityEngine;

namespace Gameplay.Board
{
    public class BoardBoosterRequest
    {
        public BoardTower TargetBoardTower { get; set; }
        public Vector2Int? TargetCell { get; set; }

        public static BoardBoosterRequest ForTower(BoardTower tower)
        {
            return new BoardBoosterRequest { TargetBoardTower = tower };
        }

        public static BoardBoosterRequest ForCell(Vector2Int cellPosition)
        {
            return new BoardBoosterRequest { TargetCell = cellPosition };
        }
    }
}
