using Core.Interfaces;
using Core.Managers;
using UI.Models;

namespace Services
{
    public class GameResultService : IGameResultService
    {
        private readonly LevelManager _levelManager;
        private readonly GameOverModel _gameOverModel;

        public GameResultService(LevelManager levelManager, GameOverModel gameOverModel)
        {
            _levelManager = levelManager;
            _gameOverModel = gameOverModel;
        }

        public void HandleWin()
        {
            _gameOverModel.ShowWinPanel();
            _levelManager.CompleteLevel();
        }

        public void HandleLose()
        {
            _gameOverModel.ShowLosePanel();
            _levelManager.CompleteLevel();
        }
    }
}