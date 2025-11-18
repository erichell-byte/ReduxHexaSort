using System;
using Hexa.Domain.Rules;
using Infrastructure.Services;

namespace UI.Models
{
    public class ScoreStorage : IScoreTracker
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnTargetScoreChanged;
        
        private readonly ISaveGameService _saveGameService;
        private int _score;
        private int _targetScore;

        public ScoreStorage(ISaveGameService saveGameService)
        {
            _saveGameService = saveGameService;
            _score = _saveGameService.LoadScore(0);
        }

        public int Score => _score;
        public int TargetScore => _targetScore;

        public void SetTargetScore(int targetScore)
        {
            if (_targetScore == targetScore) return;
            _targetScore = targetScore;
            OnTargetScoreChanged?.Invoke(_targetScore);
        }

        public void ResetScore()
        {
            SetScore(0);
        }

        public void AddScore(int amount)
        {
            SetScore(_score + amount);
        }

        private void SetScore(int value)
        {
            if (_score == value) return;
            _score = value;
            _saveGameService.SaveScore(_score);
            OnScoreChanged?.Invoke(_score);
        }
    }
}
