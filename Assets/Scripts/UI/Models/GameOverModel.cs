using System;

namespace UI.Models
{
    public class GameOverModel
    {
        public event Action<bool> OnShowPanel;
        public event Action OnHidePanel;

        public void ShowWinPanel() => OnShowPanel?.Invoke(true);
        public void ShowLosePanel() => OnShowPanel?.Invoke(false);
        public void HidePanel() => OnHidePanel?.Invoke();
    }
}