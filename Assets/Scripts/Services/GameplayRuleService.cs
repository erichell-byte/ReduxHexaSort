using Core.Interfaces;
using Hexa.Domain.Board;
using Hexa.Domain.Rules;

namespace Services
{
    public class GameplayRuleService : IGameplayRuleEvaluator
    {
        private readonly IGameResultService _resultService;

        public GameplayRuleService(IGameResultService resultService)
        {
            _resultService = resultService;
        }

        public void EvaluateScore(int newScore, int targetScore)
        {
            if (newScore >= targetScore)
            {
                _resultService.HandleWin();
            }
        }

        public void EvaluateBoard(IHexGrid grid)
        {
            bool hasEmpty = false;

            foreach (var cell in grid.Cells.Values)
            {
                if (cell.IsEmpty)
                {
                    hasEmpty = true;
                    break;
                }
            }

            if (!hasEmpty)
            {
                _resultService.HandleLose();
            }
        }
    }
}
