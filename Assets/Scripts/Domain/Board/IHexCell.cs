using UnityEngine;

namespace Hexa.Domain.Board
{
    public interface IHexCell
    {
        Vector2Int Coordinates { get; }
        IHexStack Stack { get; }
        bool IsEmpty { get; }
        void AddColor(int colorId);
        int RemoveTopColor();
        int? GetTopColor();
        void Clear();
    }
}
