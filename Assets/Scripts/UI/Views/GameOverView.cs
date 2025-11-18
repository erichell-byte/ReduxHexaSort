using TMPro;
using Presentation.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _mainMenuButton;

        private GameOverPresenter _presenter;

        [Inject]
        public void Construct(GameOverPresenter presenter)
        {
            _presenter = presenter;
        }

        private void Start()
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
            _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);

            _presenter.OnShowPanel += OnShowPanel;
            _presenter.OnHidePanel += OnHidePanel;

            HidePanel();
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveAllListeners();
            _nextLevelButton.onClick.RemoveAllListeners();
            _mainMenuButton.onClick.RemoveAllListeners();

            _presenter.OnShowPanel -= OnShowPanel;
            _presenter.OnHidePanel -= OnHidePanel;
        }

        private void OnShowPanel(bool isWin)
        {
            _resultText.text = _presenter.ResultText;
            _nextLevelButton.gameObject.SetActive(_presenter.ShowNextButton);
            _restartButton.gameObject.SetActive(!isWin);
            ShowPanel();
        }

        private void OnHidePanel() => HidePanel();
        private void ShowPanel() => _panel.SetActive(true);
        private void HidePanel() => _panel.SetActive(false);

        private void OnRestartClicked()
        {
            _presenter.RestartLevel();
            HidePanel();
        }

        private void OnNextLevelClicked()
        {
            _presenter.NextLevel();
            HidePanel();
        }

        private void OnMainMenuClicked()
        {
            _presenter.GoToMainMenu();
        }
    }
}
