using System;

namespace Hexa.Domain.Rules
{
    public interface IScoreTracker
    {
        event Action<int> OnScoreChanged;
        event Action<int> OnTargetScoreChanged;
        
        int Score { get; }
        int TargetScore { get; }
        
        void SetTargetScore(int targetScore);
        void ResetScore();
        void AddScore(int amount);
    }
}
