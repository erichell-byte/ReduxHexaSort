using UnityEngine;

namespace Gameplay.Board
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Puzzle/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public int HexCount;
        public int ColorsCount;
        public float StackSpawnChance;
        public int MaxTowerHeight;
        public ColorPalette ColorPalette;
        public int MatchCount;
        public int TargetScore;
    }
}