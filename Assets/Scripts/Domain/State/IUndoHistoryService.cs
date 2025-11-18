namespace Hexa.Domain.State
{
    public interface IUndoHistoryService
    {
        bool CanUndo { get; }
        void Clear();
        void Push(BoardSnapshot snapshot);
        BoardSnapshot Pop();
    }
}
