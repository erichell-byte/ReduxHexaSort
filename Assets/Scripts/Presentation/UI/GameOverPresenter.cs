using System;
using Core.Managers;
using UI.Models;

namespace Presentation.UI
{
    public class GameOverPresenter
    {
        private readonly GameOverModel _model;
        private readonly LevelManager _levelManager;

        public event Action<bool> OnShowPanel;
        public event Action OnHidePanel;
        public event Action RestartRequested;
        public event Action NextLevelRequested;
        public event Action MainMenuRequested;

        public bool IsWin { get; private set; }
        public bool ShowNextButton => IsWin && _levelManager.HasNextLevel();
        public string ResultText => IsWin ? "YOU WIN!" : "YOU LOSE!";

        public GameOverPresenter(GameOverModel model, LevelManager levelManager)
        {
            _model = model;
            _levelManager = levelManager;

            _model.OnShowPanel += OnModelShowPanel;
            _model.OnHidePanel += OnModelHidePanel;
        }

        public void ShowWin() => _model.ShowWinPanel();
        public void ShowLose() => _model.ShowLosePanel();
        public void Hide() => _model.HidePanel();

        public void RestartLevel()
        {
            RestartRequested?.Invoke();
        }

        public void NextLevel()
        {
            NextLevelRequested?.Invoke();
        }
        
        public void GoToMainMenu()
        {
            MainMenuRequested?.Invoke();
        }

        private void OnModelShowPanel(bool isWin)
        {
            IsWin = isWin;
            OnShowPanel?.Invoke(isWin);
        }

        private void OnModelHidePanel()
        {
            OnHidePanel?.Invoke();
        }
    }
}
