using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Gameplay.Board;

namespace Hexa.Domain.Boosters
{
    public interface IBoosterCommand
    {
        BoosterType Type { get; }
        bool CanExecute(IBoardBoosterContext context, BoardBoosterRequest request);
        UniTask<bool> ExecuteAsync(IBoardBoosterContext context, BoardBoosterRequest request);
    }
}
