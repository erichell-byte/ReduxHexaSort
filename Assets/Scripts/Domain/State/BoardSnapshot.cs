using System.Collections.Generic;
using UnityEngine;

namespace Hexa.Domain.State
{
    public class BoardSnapshot
    {
        public Dictionary<Vector2Int, List<int>> BoardState { get; }
        public List<PlayerTowerSnapshot> PlayerTowers { get; }
        public int Score { get; }

        public BoardSnapshot(Dictionary<Vector2Int, List<int>> boardState, List<PlayerTowerSnapshot> playerTowers, int score)
        {
            BoardState = boardState;
            PlayerTowers = playerTowers;
            Score = score;
        }
    }

    public class PlayerTowerSnapshot
    {
        public List<int> Colors { get; }
        public int SlotIndex { get; }
        public Vector3 Position { get; }

        public PlayerTowerSnapshot(List<int> colors, int slotIndex, Vector3 position)
        {
            Colors = colors;
            SlotIndex = slotIndex;
            Position = position;
        }
    }
}
