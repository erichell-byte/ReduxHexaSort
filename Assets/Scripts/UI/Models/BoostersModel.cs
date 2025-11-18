using System;
using System.Collections.Generic;
using Gameplay.Board;

namespace UI.Models
{
    public class BoostersModel
    {
        public event Action<BoosterType, bool> OnAvailabilityChanged;
        public event Action<BoosterType> OnBoosterUsed;

        private readonly Dictionary<BoosterType, bool> _availability = new();

        public BoostersModel()
        {
            foreach (BoosterType type in Enum.GetValues(typeof(BoosterType)))
            {
                _availability[type] = false;
            }
        }

        public bool IsAvailable(BoosterType type)
        {
            return _availability.TryGetValue(type, out var value) && value;
        }

        public void SetAvailability(BoosterType type, bool isAvailable)
        {
            if (_availability.TryGetValue(type, out var current) && current == isAvailable)
                return;

            _availability[type] = isAvailable;
            OnAvailabilityChanged?.Invoke(type, isAvailable);
        }

        public void NotifyBoosterUsed(BoosterType type)
        {
            OnBoosterUsed?.Invoke(type);
        }
    }
}
