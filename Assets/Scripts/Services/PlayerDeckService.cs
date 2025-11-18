using Gameplay.Board;
using Hexa.Domain.Board;
using Hexa.Domain.Player;
using UnityEngine;

namespace Services
{
    public class PlayerDeckService : IPlayerDeckService
    {
        private readonly LevelConfig _levelConfig;

        public PlayerDeckService(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
        }

        public IHexCell CreateRandomTower()
        {
            var cell = new HexCell(new Vector2Int(-1, -1));
            int height = Random.Range(1, _levelConfig.MaxTowerHeight + 1);

            for (int i = 0; i < height; i++)
            {
                cell.AddColor(Random.Range(0, _levelConfig.ColorsCount));
            }

            return cell;
        }
    }
}
