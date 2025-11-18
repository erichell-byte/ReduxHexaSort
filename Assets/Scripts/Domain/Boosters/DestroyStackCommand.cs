using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Gameplay.Board;

namespace Hexa.Domain.Boosters
{
    public class DestroyStackCommand : IBoosterCommand
    {
        public BoosterType Type => BoosterType.DestroyTower;

        public bool CanExecute(IBoardBoosterContext context, BoardBoosterRequest request)
        {
            if (context == null) return false;
            if (request == null) return false;
            return request.TargetBoardTower != null || request.TargetCell.HasValue;
        }

        public UniTask<bool> ExecuteAsync(IBoardBoosterContext context, BoardBoosterRequest request)
        {
            if (context == null || request == null)
                return UniTask.FromResult(false);

            if (request.TargetBoardTower != null)
            {
                bool result = context.TryDestroyTower(request.TargetBoardTower);
                return UniTask.FromResult(result);
            }

            if (request.TargetCell.HasValue)
            {
                bool result = context.TryDestroyTower(request.TargetCell.Value);
                return UniTask.FromResult(result);
            }

            return UniTask.FromResult(false);
        }
    }
}
