using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.GameStates
{
    public interface IGameFlowService
    {
        UniTask EnterMenuAsync(CancellationToken token = default);
        UniTask EnterGameplayAsync(CancellationToken token = default);
        UniTask EnterLoadingAsync(CancellationToken token = default);
        UniTask RestartLevelAsync(CancellationToken token = default);
    }
}
