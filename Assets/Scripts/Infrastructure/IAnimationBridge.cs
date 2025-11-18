using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services
{
    public interface IAnimationBridge
    {
        Task AnimatePieceTransfer(Transform piece, Vector3 targetPosition);
        Task AnimatePieceRemoval(Transform piece);
    }
}
