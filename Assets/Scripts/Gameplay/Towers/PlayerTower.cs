using System.Threading.Tasks;
using DG.Tweening;
using Gameplay.Board;
using Hexa.Domain.Board;
using UnityEngine;

namespace Gameplay.Towers
{
    public class PlayerTower : HexTower
    {
        private Vector3 _startPosition;
        private IHexCell _cellModel;
        private int _slotIndex = -1;

        public Vector3 StartPosition => _startPosition;
        public int SlotIndex => _slotIndex;
        public IHexCell CellModel => _cellModel;
        
        public void Initialize(IHexCell cell, Vector3 position, ColorPalette colorPalette, int slotIndex = -1)
        {
            _cellModel = cell;
            _startPosition = position;
            _slotIndex = slotIndex;
            _colorPalette = colorPalette;
        
            transform.position = position;
            CreateTowerVisual(_cellModel.Stack.Colors);
            PlaySpawnAnimation();
            AddCollider();
        }
        
        private void AddCollider()
        {
            var collider = gameObject.AddComponent<BoxCollider>();
            float height = _cellModel.Stack.Count * _pieceHeight;
            collider.size = new Vector3(1.5f, height, 1.5f);
            collider.center = new Vector3(0, height * 0.5f, 0);
        }
        
        public async Task ReturnToStartPosition()
        {
            await transform.DOMove(_startPosition, 0.3f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
        
        public async Task AnimateToPosition(Vector3 targetPosition)
        {
            transform.DOKill();
        
            await transform.DOMove(targetPosition, 0.4f)
                .SetEase(Ease.OutCubic)
                .AsyncWaitForCompletion();
        }
    }
}
