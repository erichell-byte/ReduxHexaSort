using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.GameStates
{
    public class GameStateMachine : IInitializable
    {
        private readonly List<GameState> _states;
        private GameState _currentState;

        public GameStateMachine(List<GameState> states)
        {
            _states = states;
        }

        public void Initialize()
        {
            ChangeStateAsync<LoadingState>().Forget();
        }

        public async UniTask ChangeStateAsync<T>(CancellationToken token = default) where T : GameState
        {
            token = token == default ? CancellationToken.None : token;

            if (_currentState != null)
            {
                await _currentState.ExitAsync(token);
            }

            _currentState = GetState<T>();
            await _currentState.EnterAsync(token);
        }

        private GameState GetState<T>() where T : GameState
        {
            foreach (var state in _states)
            {
                if (state is T)
                    return state;
            }
            throw new Exception($"State {typeof(T)} not found");
        }
    }
}
