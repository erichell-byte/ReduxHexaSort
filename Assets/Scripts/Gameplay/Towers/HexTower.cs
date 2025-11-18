using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Board;
using UnityEngine;

namespace Gameplay.Towers
{
    public abstract class HexTower : MonoBehaviour
    {
        [SerializeField] protected GameObject _hexPiecePrefab;
        
        protected ColorPalette _colorPalette;
        protected float _pieceHeight = 0.2f;
        
        private List<GameObject> _visualPieces = new List<GameObject>();

        public Transform GetTopPieceVisual()
        {
            if (_visualPieces.Count > 0)
            {
                return _visualPieces[_visualPieces.Count - 1].transform;
            }
            return null;
        }
        
        protected void CreateTowerVisual(IEnumerable<int> colorIds)
        {
            ClearVisualPieces();

            int pieceIndex = 0;
            foreach (var colorId in colorIds)
            {
                var pieceObj = Instantiate(_hexPiecePrefab, transform);
                float yPos = pieceIndex * _pieceHeight;
                pieceObj.transform.localPosition = new Vector3(0, yPos, 0);
        
                SetPieceColor(pieceObj, colorId);
                _visualPieces.Add(pieceObj);
                pieceIndex++;
            }
        }

        private void SetPieceColor(GameObject pieceObj, int colorId)
        {
            var meshRenderers = pieceObj.GetComponent<CellView>().Renderers;
            if (meshRenderers != null && _colorPalette != null && colorId < _colorPalette.Colors.Length)
            {
                foreach (var meshRenderer in meshRenderers)
                    meshRenderer.material.color = _colorPalette.Colors[colorId];
            }
        }

        private void ClearVisualPieces()
        {
            foreach (var piece in _visualPieces)
            {
                if (piece != null) Destroy(piece);
            }
            _visualPieces.Clear();
        }
        
        protected Vector3 CalculateWorldPosition(Vector2Int gridPos)
        {
            float x = gridPos.x * 1.5f;
            float z = gridPos.y * 1.732f + gridPos.x * 0.866f;
            return new Vector3(x, 0, z);
        }
        
        protected void PlaySpawnAnimation()
        {
            transform.localScale = Vector3.one * 0.1f;
            transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack);
        }
    }
}