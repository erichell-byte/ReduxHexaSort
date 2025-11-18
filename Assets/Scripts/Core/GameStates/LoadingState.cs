using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.GameStates
{
    public class LoadingState : GameState
    {
        private readonly IGameFlowService _gameFlowService;

        public LoadingState(IGameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }

        public override UniTask EnterAsync(CancellationToken token)
        {
            return _gameFlowService.EnterMenuAsync(token);
        }

        public override UniTask ExitAsync(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}
