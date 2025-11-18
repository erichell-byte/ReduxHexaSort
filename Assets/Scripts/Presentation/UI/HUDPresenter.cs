using System;
using Gameplay.Board;
using Hexa.Domain.Rules;

namespace Presentation.UI
{
    public class HUDPresenter
    {
        private readonly IScoreTracker _scoreTracker;
        
        public event Action<int> OnScoreChanged;
        public event Action<int> OnTargetScoreChanged;
        
        public int Score => _scoreTracker.Score;
        public int TargetScore => _scoreTracker.TargetScore;
        public float Progress => _scoreTracker.TargetScore > 0 ? (float)_scoreTracker.Score / _scoreTracker.TargetScore : 0f;

        public HUDPresenter(IScoreTracker scoreTracker, LevelConfig levelConfig)
        {
            _scoreTracker = scoreTracker;
            
            _scoreTracker.OnScoreChanged += score => OnScoreChanged?.Invoke(score);
            _scoreTracker.OnTargetScoreChanged += target => OnTargetScoreChanged?.Invoke(target);
            
            _scoreTracker.SetTargetScore(levelConfig.TargetScore);
            _scoreTracker.ResetScore();
        }

        public void AddScore(int amount)
        {
            _scoreTracker.AddScore(amount);
        }
    }
}
