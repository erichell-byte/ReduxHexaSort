using Gameplay.Board;
using System;
using Hexa.Domain.Board;
using UnityEngine;

namespace Gameplay.Towers
{
    public class BoardTower : HexTower
    {
        private IHexCell _cellModel;
        private Vector2Int GridPosition { get; set; }
        private Action<BoardTower> _onSelected;
        
        public virtual int? GetTopColor()
        {
            return _cellModel?.GetTopColor();
        }
        
        public void Initialize(IHexCell cell, ColorPalette colorPalette)
        {
            _cellModel = cell;
            GridPosition = cell.Coordinates;
            _colorPalette = colorPalette;
        
            Vector3 position = CalculateWorldPosition(GridPosition);
            transform.position = position;
        
            CreateTowerVisual(_cellModel.Stack.Colors);
            PlaySpawnAnimation();
        }

        public virtual void RefreshVisual()
        {
            if (this == null || _cellModel == null)
            {
                return;
            }

            CreateTowerVisual(_cellModel.Stack.Colors);
        }

        public void BindSelection(Action<BoardTower> onSelected)
        {
            _onSelected = onSelected;
        }

        private void OnMouseDown()
        {
            _onSelected?.Invoke(this);
        }

        public bool IsEmpty => _cellModel?.IsEmpty ?? true;
    }
}
