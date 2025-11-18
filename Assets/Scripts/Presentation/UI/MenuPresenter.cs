using System;
using Infrastructure.Services;
using UI.Models;
using UnityEngine;

namespace Presentation.UI
{
    public class MenuPresenter
    {
        private readonly MenuModel _menuModel;
        private readonly ISaveGameService _saveGameService;

        public event Action PlayRequested;
        public event Action ClearProgressRequested;

        public MenuPresenter(MenuModel menuModel,
            ISaveGameService saveGameService)
        {
            _menuModel = menuModel;
            _saveGameService = saveGameService;
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
            ClearProgressRequested?.Invoke();
        }
    }
}
