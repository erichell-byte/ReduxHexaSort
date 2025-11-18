using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Infrastructure.Services
{
    public class AnimationBridge : IAnimationBridge
    {
        public async Task AnimatePieceTransfer(Transform piece, Vector3 targetPosition)
        {
            if (piece == null) return;

            piece.SetParent(null);
            Vector3 startPosition = piece.position;
            Vector3 midPoint = Vector3.Lerp(startPosition, targetPosition, 0.5f) + Vector3.up * 0.75f;
            Vector3 originalScale = piece.localScale;

            var sequence = DOTween.Sequence();
            sequence.Append(piece.DOMove(midPoint, 0.25f).SetEase(Ease.OutCubic));
            sequence.Append(piece.DOMove(targetPosition, 0.25f).SetEase(Ease.InQuad));
            sequence.Join(piece.DORotate(new Vector3(0f, 360f, 0f), 0.5f, RotateMode.FastBeyond360));
            sequence.Join(piece.DOScale(originalScale * 1.3f, 0.5f).SetLoops(2, LoopType.Yoyo));

            await sequence.AsyncWaitForCompletion();

            if (piece != null)
            {
                piece.rotation = Quaternion.identity;
                piece.localScale = originalScale;
                Object.Destroy(piece.gameObject);
            }
        }

        public async Task AnimatePieceRemoval(Transform piece)
        {
            if (piece == null) return;

            piece.SetParent(null);
            var sequence = DOTween.Sequence();
            sequence.Append(piece.DOMove(piece.position + Vector3.up * 0.4f, 0.15f).SetEase(Ease.OutQuad));
            sequence.Join(piece.DORotate(new Vector3(360f, 360f, 0f), 0.3f, RotateMode.FastBeyond360));
            sequence.Append(piece.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));

            await sequence.AsyncWaitForCompletion();

            if (piece != null && piece.gameObject != null)
            {
                Object.Destroy(piece.gameObject);
            }
        }
    }
}
