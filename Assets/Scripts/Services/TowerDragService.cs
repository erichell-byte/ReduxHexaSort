using System;
using DG.Tweening;
using Gameplay.Board;
using Gameplay.Towers;
using Infrastructure.Services;
using UnityEngine;

namespace Services
{
    public class TowerDragService : IDisposable
    {
        private readonly BoardController _boardController;
        private readonly ITowerInputService _inputService;
        private PlayerTower _activeTower;

        public TowerDragService(BoardController boardController, ITowerInputService inputService)
        {
            _boardController = boardController;
            _inputService = inputService;

            _inputService.OnDragStarted += HandleDragStarted;
            _inputService.OnDragging += HandleDragging;
            _inputService.OnDragEnded += HandleDragEnded;
        }

        public void Dispose()
        {
            _inputService.OnDragStarted -= HandleDragStarted;
            _inputService.OnDragging -= HandleDragging;
            _inputService.OnDragEnded -= HandleDragEnded;
        }

        private void HandleDragStarted(PlayerTower tower, Vector3 position)
        {
            _activeTower = tower;
            tower.transform.DOScale(1.1f, 0.1f);
            tower.transform.position = position + Vector3.up * 0.5f;
        }

        private void HandleDragging(PlayerTower tower, Vector3 position)
        {
            if (_activeTower != tower) return;
            tower.transform.position = position + Vector3.up * 0.5f;
        }

        private void HandleDragEnded(PlayerTower tower, Vector3 position)
        {
            if (_activeTower != tower) return;

            tower.transform.DOScale(1f, 0.1f);
            _boardController.TryPlacePlayerTower(tower, tower.CellModel, tower.transform.position);
            _activeTower = null;
        }
    }
}
