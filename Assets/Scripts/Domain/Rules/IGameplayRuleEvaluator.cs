using Hexa.Domain.Board;

namespace Hexa.Domain.Rules
{
    public interface IGameplayRuleEvaluator
    {
        void EvaluateScore(int currentScore, int targetScore);
        void EvaluateBoard(IHexGrid grid);
    }
}
