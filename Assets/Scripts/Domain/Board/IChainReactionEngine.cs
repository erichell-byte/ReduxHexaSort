using System.Threading.Tasks;
using UnityEngine;

namespace Hexa.Domain.Board
{
    public interface IChainReactionEngine
    {
        Task CheckChains(Vector2Int placedPosition);
    }
}
