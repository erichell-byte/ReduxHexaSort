using System;
using Core.Managers;
using Infrastructure.Services;
using UI.Models;
using UnityEngine;

namespace Presentation.UI
{
    public class MenuPresenter
    {
        private readonly MenuModel _menuModel;
        private readonly ISaveGameService _saveGameService;
        private readonly LevelManager _levelManager;

        public event Action PlayRequested;
        public event Action ClearProgressRequested;

        public MenuPresenter(MenuModel menuModel,
            ISaveGameService saveGameService,
            LevelManager levelManager)
        {
            _menuModel = menuModel;
            _saveGameService = saveGameService;
            _levelManager = levelManager;
        }

        public void OnPlayClicked()
        {
            _menuModel.RequestPlay();
            PlayRequested?.Invoke();
        }

        public void OnQuitClicked()
        {
            _menuModel.RequestQuit();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnClearProgressClicked()
        {
            _saveGameService.ResetProgress();
            _levelManager.ResetProgress();
            ClearProgressRequested?.Invoke();
        }
    }
}
