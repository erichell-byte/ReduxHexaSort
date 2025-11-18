using UnityEngine;

namespace Gameplay.Board
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "Puzzle/Color Palette")]
    public class ColorPalette : ScriptableObject
    {
        public Color[] Colors;
    }
}