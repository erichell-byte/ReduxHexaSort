using System;
using Gameplay.Towers;
using UnityEngine;

namespace Infrastructure.Services
{
    public interface ITowerInputService
    {
        event Action<PlayerTower, Vector3> OnDragStarted;
        event Action<PlayerTower, Vector3> OnDragging;
        event Action<PlayerTower, Vector3> OnDragEnded;
    }
}
