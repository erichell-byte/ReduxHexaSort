using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Gameplay.Board;
using Hexa.Domain.Boosters;

namespace Services
{
    public class BoosterExecutor : IBoosterExecutor
    {
        private readonly IBoosterCatalog _catalog;
        private readonly IBoardBoosterContext _context;

        public BoosterExecutor(IBoosterCatalog catalog, IBoardBoosterContext context)
        {
            _catalog = catalog;
            _context = context;
        }

        public bool CanExecute(BoosterType type, BoardBoosterRequest request = null)
        {
            if (!_catalog.TryGetCommand(type, out var command)) return false;
            request ??= new BoardBoosterRequest();
            return command.CanExecute(_context, request);
        }

        public async UniTask<bool> ExecuteAsync(BoosterType type, BoardBoosterRequest request = null)
        {
            if (!_catalog.TryGetCommand(type, out var command))
                return false;

            request ??= new BoardBoosterRequest();

            if (!command.CanExecute(_context, request))
                return false;

            return await command.ExecuteAsync(_context, request);
        }
    }
}
