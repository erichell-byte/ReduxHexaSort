using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Board
{
    [CreateAssetMenu(menuName = "Puzzle/Level List")]
    public class Levels : ScriptableObject
    {
        public List<LevelConfig> LevelConfigs;
    }
}