using Gameplay.Board;
using Presentation.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class BoostersView : MonoBehaviour
    {
        [SerializeField] private Button _undoButton;
        [SerializeField] private Button _destroyButton;
        [SerializeField] private Button _shuffleButton;

        private BoosterPresenter _presenter;

        [Inject]
        public void Construct(BoosterPresenter presenter)
        {
            _presenter = presenter;
        }

        private void Start()
        {
            _undoButton.onClick.AddListener(OnUndoClicked);
            _destroyButton.onClick.AddListener(OnDestroyClicked);
            _shuffleButton.onClick.AddListener(OnShuffleClicked);

            _presenter.OnAvailabilityChanged += OnAvailabilityChanged;

            RefreshButtons();
        }

        private void OnDestroy()
        {
            _undoButton.onClick.RemoveListener(OnUndoClicked);
            _destroyButton.onClick.RemoveListener(OnDestroyClicked);
            _shuffleButton.onClick.RemoveListener(OnShuffleClicked);

            if (_presenter != null)
            {
                _presenter.OnAvailabilityChanged -= OnAvailabilityChanged;
            }
        }

        private void OnUndoClicked() => _presenter.UseUndoBooster();
        private void OnDestroyClicked() => _presenter.UseDestroyBooster();
        private void OnShuffleClicked() => _presenter.UseShuffleBooster();

        private void RefreshButtons()
        {
            UpdateButton(BoosterType.UndoMove, _presenter.IsBoosterAvailable(BoosterType.UndoMove));
            UpdateButton(BoosterType.DestroyTower, _presenter.IsBoosterAvailable(BoosterType.DestroyTower));
            UpdateButton(BoosterType.ShuffleBoard, _presenter.IsBoosterAvailable(BoosterType.ShuffleBoard));
        }

        private void OnAvailabilityChanged(BoosterType type, bool available)
        {
            UpdateButton(type, available);
        }

        private void UpdateButton(BoosterType type, bool available)
        {
            switch (type)
            {
                case BoosterType.UndoMove:
                    if (_undoButton != null) _undoButton.interactable = available;
                    break;
                case BoosterType.DestroyTower:
                    if (_destroyButton != null) _destroyButton.interactable = available;
                    break;
                case BoosterType.ShuffleBoard:
                    if (_shuffleButton != null) _shuffleButton.interactable = available;
                    break;
            }
        }
    }
}
