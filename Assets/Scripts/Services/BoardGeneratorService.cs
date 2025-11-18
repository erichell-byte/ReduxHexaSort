using System.Collections.Generic;
using Core.Interfaces;
using Gameplay.Board;
using Hexa.Domain.Board;
using UnityEngine;

namespace Services
{
    public class BoardGeneratorService : IBoardGeneratorService
    {
        public IHexGrid GenerateBoard(LevelConfig config)
        {
            var cells = new Dictionary<Vector2Int, IHexCell>();
            var positions = GenerateRadialPositions(config.HexCount);
            
            foreach (var pos in positions)
            {
                var cell = new HexCell(pos);
                
                if (Random.Range(0f, 1f) < config.StackSpawnChance)
                {
                    int towerHeight = Random.Range(1, config.MaxTowerHeight + 1);
                    
                    for (int i = 0; i < towerHeight; i++)
                    {
                        cell.AddColor(Random.Range(0, config.ColorsCount));
                    }
                }
                
                cells[pos] = cell;
            }
            
            return new HexGrid(cells);
        }

        public Vector3 CalculateWorldPosition(Vector2Int gridPos)
        {
            float x = gridPos.x * 1.5f;
            float z = gridPos.y * 1.732f + gridPos.x * 0.866f;
            return new Vector3(x, -0.2f, z);
        }

        private HashSet<Vector2Int> GenerateRadialPositions(int rings)
        {
            var positions = new HashSet<Vector2Int>();
            
            var center = new Vector2Int(0, 0);
            positions.Add(center);
            
            Vector2Int[] directions = 
            {
                new(1, 0), new(1, -1), new(0, -1),
                new(-1, 0), new(-1, 1), new(0, 1)
            };
            
            for (int ring = 1; ring <= rings; ring++)
            {
                Vector2Int current = center + new Vector2Int(ring, 0);
                
                for (int side = 0; side < 6; side++)
                {
                    for (int step = 0; step < ring; step++)
                    {
                        positions.Add(current);
                        current += directions[(side + 2) % 6];
                    }
                }
            }
            
            return positions;
        }
    }
}
