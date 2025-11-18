using System.Collections.Generic;
using Gameplay.Board;

namespace Hexa.Domain.Boosters
{
    public interface IBoosterCatalog
    {
        IEnumerable<IBoosterCommand> Commands { get; }
        bool TryGetCommand(BoosterType type, out IBoosterCommand command);
    }
}
