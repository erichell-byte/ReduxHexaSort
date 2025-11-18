using Hexa.Domain.Board;

namespace Hexa.Domain.Player
{
    public interface IPlayerDeckService
    {
        IHexCell CreateRandomTower();
    }
}
