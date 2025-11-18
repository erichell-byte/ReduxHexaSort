using System.Collections.Generic;

namespace Hexa.Domain.Board
{
    public interface IHexStack
    {
        IReadOnlyList<int> Colors { get; }
        bool IsEmpty { get; }
        int Count { get; }

        void Push(int colorId);
        int RemoveTop();
        int? Peek();
        void Clear();
        void SetFrom(IEnumerable<int> colors);
    }
}
