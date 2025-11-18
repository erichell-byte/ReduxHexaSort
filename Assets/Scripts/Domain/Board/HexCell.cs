using UnityEngine;

namespace Hexa.Domain.Board
{
    public class HexCell : IHexCell
    {
        public Vector2Int Coordinates { get; }
        public IHexStack Stack { get; }
        public bool IsEmpty => Stack.IsEmpty;

        public HexCell(Vector2Int coordinates)
        {
            Coordinates = coordinates;
            Stack = new HexStack();
        }

        public HexCell(Vector2Int coordinates, IHexStack stack)
        {
            Coordinates = coordinates;
            Stack = stack;
        }

        public void AddColor(int colorId)
        {
            Stack.Push(colorId);
        }

        public int RemoveTopColor()
        {
            return Stack.RemoveTop();
        }

        public int? GetTopColor()
        {
            return Stack.Peek();
        }

        public void Clear()
        {
            Stack.Clear();
        }
    }
}
