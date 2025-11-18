using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Gameplay.Board;

namespace Hexa.Domain.Boosters
{
    public class ShuffleBoardCommand : IBoosterCommand
    {
        public BoosterType Type => BoosterType.ShuffleBoard;

        public bool CanExecute(IBoardBoosterContext context, BoardBoosterRequest request)
        {
            return context != null;
        }

        public UniTask<bool> ExecuteAsync(IBoardBoosterContext context, BoardBoosterRequest request)
        {
            if (context == null) return UniTask.FromResult(false);

            bool result = context.TryShuffleBoard();
            return UniTask.FromResult(result);
        }
    }
}
