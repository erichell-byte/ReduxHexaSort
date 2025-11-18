using System.Collections.Generic;
using System.Linq;
using Gameplay.Board;
using Hexa.Domain.Boosters;

namespace Services
{
    public class BoosterCatalog : IBoosterCatalog
    {
        private readonly Dictionary<BoosterType, IBoosterCommand> _commands;

        public BoosterCatalog(IEnumerable<IBoosterCommand> commands)
        {
            _commands = commands.ToDictionary(c => c.Type);
        }

        public IEnumerable<IBoosterCommand> Commands => _commands.Values;

        public bool TryGetCommand(BoosterType type, out IBoosterCommand command)
        {
            return _commands.TryGetValue(type, out command);
        }
    }
}
