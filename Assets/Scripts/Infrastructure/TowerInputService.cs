using System;
using Gameplay.Towers;
using UnityEngine;

namespace Infrastructure.Services
{
    public class TowerInputService : MonoBehaviour, ITowerInputService
    {
        public event Action<PlayerTower, Vector3> OnDragStarted;
        public event Action<PlayerTower, Vector3> OnDragging;
        public event Action<PlayerTower, Vector3> OnDragEnded;

        private PlayerTower _activeTower;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryBeginDrag();
            }
            else if (_activeTower != null && Input.GetMouseButton(0))
            {
                if (TryGetPointerWorldPosition(out var position))
                {
                    OnDragging?.Invoke(_activeTower, position);
                }
            }
            else if (_activeTower != null && Input.GetMouseButtonUp(0))
            {
                if (TryGetPointerWorldPosition(out var position))
                {
                    OnDragEnded?.Invoke(_activeTower, position);
                }
                _activeTower = null;
            }
        }

        private void TryBeginDrag()
        {
            if (!TryGetPointerWorldPosition(out var position))
                return;

            var tower = RaycastForTower();
            if (tower == null) return;

            _activeTower = tower;
            OnDragStarted?.Invoke(tower, position);
        }

        private PlayerTower RaycastForTower()
        {
            var camera = Camera.main;
            if (camera == null) return null;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                return hit.collider.GetComponent<PlayerTower>();
            }

            return null;
        }

        private bool TryGetPointerWorldPosition(out Vector3 position)
        {
            position = default;
            var camera = Camera.main;
            if (camera == null) return false;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float distance))
            {
                position = ray.GetPoint(distance);
                return true;
            }

            return false;
        }
    }
}
