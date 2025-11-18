using System.Collections.Generic;
using Gameplay.Board;
using Infrastructure.Services;
using UnityEngine;

namespace Core.Managers
{
    public class LevelManager
    {
        private readonly List<LevelConfig> _levels;
        private readonly ISaveGameService _saveGameService;
        private int _currentLevelIndex ;
        private bool _isLevelActive ;

        public LevelConfig CurrentLevel => _levels[_currentLevelIndex];
        private bool IsLastLevel => _currentLevelIndex >= _levels.Count - 1;

        public LevelManager(List<LevelConfig> levels, ISaveGameService saveGameService)
        {
            _levels = levels;
            _saveGameService = saveGameService;
            _currentLevelIndex = Mathf.Clamp(_saveGameService.LoadCurrentLevel(0), 0, Mathf.Max(0, _levels.Count - 1));
        }

        private void SetLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= _levels.Count)
            {
                return;
            }

            _currentLevelIndex = levelIndex;
            _saveGameService?.SaveCurrentLevel(_currentLevelIndex);
        }

        public void CompleteLevel()
        {
            if (!_isLevelActive) return;

            _isLevelActive = false;
        }

        public void BeginLevel()
        {
            _isLevelActive = true;
        }

        public void RestartCurrentLevel() => SetLevel(_currentLevelIndex);

        public void NextLevel() => SetLevel(_currentLevelIndex + 1);

        public bool HasNextLevel() => !IsLastLevel;
    }
}
