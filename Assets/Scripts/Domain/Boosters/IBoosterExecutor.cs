using Cysharp.Threading.Tasks;
using Gameplay.Board;

namespace Hexa.Domain.Boosters
{
    public interface IBoosterExecutor
    {
        UniTask<bool> ExecuteAsync(BoosterType type, BoardBoosterRequest request = null);
        bool CanExecute(BoosterType type, BoardBoosterRequest request = null);
    }
}
