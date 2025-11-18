using System.Collections.Generic;
using System.Linq;

namespace Hexa.Domain.Board
{
    public class HexStack : IHexStack
    {
        private readonly List<int> _colors = new();

        public IReadOnlyList<int> Colors => _colors;
        public bool IsEmpty => _colors.Count == 0;
        public int Count => _colors.Count;

        public void Push(int colorId)
        {
            _colors.Add(colorId);
        }

        public int RemoveTop()
        {
            if (_colors.Count == 0)
                return -1;

            int top = _colors[_colors.Count - 1];
            _colors.RemoveAt(_colors.Count - 1);
            return top;
        }

        public int? Peek()
        {
            if (_colors.Count == 0)
                return null;
            return _colors[_colors.Count - 1];
        }

        public void Clear()
        {
            _colors.Clear();
        }

        public void SetFrom(IEnumerable<int> colors)
        {
            _colors.Clear();
            if (colors == null) return;
            _colors.AddRange(colors);
        }
    }
}
