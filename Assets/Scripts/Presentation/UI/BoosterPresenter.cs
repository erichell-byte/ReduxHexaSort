using System;
using Gameplay.Board;
using Hexa.Domain.Boosters;
using UI.Models;

namespace Presentation.UI
{
    public class BoosterPresenter
    {
        private readonly BoostersModel _model;
        private readonly IBoosterExecutor _executor;

        private Gameplay.Towers.BoardTower _destroyTargetTower;
        private UnityEngine.Vector2Int? _destroyTargetCell;
        private bool _awaitingDestroySelection;

        public BoosterPresenter(BoostersModel model, IBoosterExecutor executor)
        {
            _model = model;
            _executor = executor;

            RefreshAvailability();
        }

        public event Action<BoosterType, bool> OnAvailabilityChanged
        {
            add => _model.OnAvailabilityChanged += value;
            remove => _model.OnAvailabilityChanged -= value;
        }

        public event Action<BoosterType> OnBoosterUsed
        {
            add => _model.OnBoosterUsed += value;
            remove => _model.OnBoosterUsed -= value;
        }

        public bool IsBoosterAvailable(BoosterType type) => _model.IsAvailable(type);

        public void SetUndoAvailability(bool available)
        {
            _model.SetAvailability(BoosterType.UndoMove, available);
        }

        public void BeginDestroySelection()
        {
            if (!_model.IsAvailable(BoosterType.DestroyTower)) return;
            _awaitingDestroySelection = true;
        }

        public void HandleBoardTowerSelection(Gameplay.Towers.BoardTower tower)
        {
            if (!_awaitingDestroySelection || tower == null) return;

            _destroyTargetTower = tower;
            _destroyTargetCell = null;
            ExecuteBooster(BoosterType.DestroyTower, BuildDestroyRequest());
            _awaitingDestroySelection = false;
        }

        public void ClearDestroyTarget()
        {
            _destroyTargetTower = null;
            _destroyTargetCell = null;
            _awaitingDestroySelection = false;
        }

        public void RefreshAvailability()
        {
            _model.SetAvailability(BoosterType.DestroyTower, true);
            _model.SetAvailability(BoosterType.ShuffleBoard, _executor.CanExecute(BoosterType.ShuffleBoard));
        }

        public void UseDestroyBooster() => BeginDestroySelection();
        public void UseUndoBooster() => ExecuteBooster(BoosterType.UndoMove);
        public void UseShuffleBooster() => ExecuteBooster(BoosterType.ShuffleBoard);

        private async void ExecuteBooster(BoosterType type, BoardBoosterRequest request = null)
        {
            if (!_model.IsAvailable(type)) return;

            bool success = await _executor.ExecuteAsync(type, request);
            if (!success) return;

            if (type == BoosterType.DestroyTower)
            {
                ClearDestroyTarget();
            }

            _model.NotifyBoosterUsed(type);
        }

        private BoardBoosterRequest BuildDestroyRequest()
        {
            if (_destroyTargetTower != null)
                return BoardBoosterRequest.ForTower(_destroyTargetTower);

            if (_destroyTargetCell.HasValue)
                return BoardBoosterRequest.ForCell(_destroyTargetCell.Value);

            return new BoardBoosterRequest();
        }
    }
}
