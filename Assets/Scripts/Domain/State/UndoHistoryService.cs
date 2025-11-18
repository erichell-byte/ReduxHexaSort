using System.Collections.Generic;

namespace Hexa.Domain.State
{
    public class UndoHistoryService : IUndoHistoryService
    {
        private readonly Stack<BoardSnapshot> _history = new();

        public bool CanUndo => _history.Count > 0;

        public void Clear()
        {
            _history.Clear();
        }

        public void Push(BoardSnapshot snapshot)
        {
            if (snapshot == null) return;
            _history.Push(snapshot);
        }

        public BoardSnapshot Pop()
        {
            if (_history.Count == 0) return null;
            return _history.Pop();
        }
    }
}
