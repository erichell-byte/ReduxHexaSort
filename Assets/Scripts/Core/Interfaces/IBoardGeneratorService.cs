using Gameplay.Board;
using Hexa.Domain.Board;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IBoardGeneratorService
    {
        IHexGrid GenerateBoard(LevelConfig config);
        Vector3 CalculateWorldPosition(Vector2Int gridPos);
    }
}
