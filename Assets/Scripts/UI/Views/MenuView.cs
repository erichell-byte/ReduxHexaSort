using Presentation.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _clearSaveButton;

        private MenuPresenter _presenter;

        [Inject]
        public void Construct(MenuPresenter presenter)
        {
            _presenter = presenter;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(() => _presenter.OnPlayClicked());
            _quitButton.onClick.AddListener(() => _presenter.OnQuitClicked());
            if (_clearSaveButton != null)
                _clearSaveButton.onClick.AddListener(() => _presenter.OnClearProgressClicked());
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            if (_clearSaveButton != null)
                _clearSaveButton.onClick.RemoveAllListeners();
        }
    }
}
