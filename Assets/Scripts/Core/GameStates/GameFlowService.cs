using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.GameStates
{
    public class GameFlowService : IGameFlowService
    {
        private readonly LazyInject<GameStateMachine> _stateMachine;

        public GameFlowService(LazyInject<GameStateMachine> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask EnterLoadingAsync(CancellationToken token = default) =>
            _stateMachine.Value.ChangeStateAsync<LoadingState>(token);

        public UniTask EnterMenuAsync(CancellationToken token = default) =>
            _stateMachine.Value.ChangeStateAsync<MenuState>(token);

        public UniTask EnterGameplayAsync(CancellationToken token = default) =>
            _stateMachine.Value.ChangeStateAsync<GameplayState>(token);

        public UniTask RestartLevelAsync(CancellationToken token = default) =>
            _stateMachine.Value.ChangeStateAsync<GameplayState>(token);
    }
}
