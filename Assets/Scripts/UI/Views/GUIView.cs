using Presentation.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Views
{
    public class GUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText; 

        private HUDPresenter _presenter;

        [Inject]
        public void Construct(HUDPresenter presenter)
        {
            _presenter = presenter;
        }

        private void Start()
        {
            _presenter.OnScoreChanged += OnScoreChanged;
            _presenter.OnTargetScoreChanged += OnTargetScoreChanged;
            
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            _scoreText.text = $"{_presenter.Score} / {_presenter.TargetScore}";
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.text = $"{score} / {_presenter.TargetScore}";
        }

        private void OnTargetScoreChanged(int targetScore)
        {
            _scoreText.text = $"{_presenter.Score} / {targetScore}";
        }
        
        private void OnDestroy()
        {
            _presenter.OnScoreChanged -= OnScoreChanged;
            _presenter.OnTargetScoreChanged -= OnTargetScoreChanged;
        }
    }
}
