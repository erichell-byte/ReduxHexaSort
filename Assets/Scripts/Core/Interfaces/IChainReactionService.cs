using System;
using System.Collections.Generic;
using Gameplay.Towers;
using Hexa.Domain.Board;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IChainReactionService : IChainReactionEngine
    {
        public event Action<int> OnScoreAdded;
        void Initialize(IHexGrid grid, Dictionary<Vector2Int, BoardTower> boardTowers);
    }
}
