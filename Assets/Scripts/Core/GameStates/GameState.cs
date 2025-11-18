using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.GameStates
{
    public abstract class GameState
    {
        public abstract UniTask EnterAsync(CancellationToken token);
        public abstract UniTask ExitAsync(CancellationToken token);
    }
}
