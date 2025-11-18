using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Gameplay.Board;

namespace Hexa.Domain.Boosters
{
    public class UndoMoveCommand : IBoosterCommand
    {
        public BoosterType Type => BoosterType.UndoMove;

        public bool CanExecute(IBoardBoosterContext context, BoardBoosterRequest request)
        {
            return context != null && context.CanUndo;
        }

        public UniTask<bool> ExecuteAsync(IBoardBoosterContext context, BoardBoosterRequest request)
        {
            if (context == null) return UniTask.FromResult(false);

            bool result = context.TryUndoLastAction();
            return UniTask.FromResult(result);
        }
    }
}
