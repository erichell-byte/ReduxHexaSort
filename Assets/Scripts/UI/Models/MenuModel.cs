using System;

namespace UI.Models
{
    public class MenuModel
    {
        public event Action PlayRequested;
        public event Action QuitRequested;

        public void RequestPlay()
        {
            PlayRequested?.Invoke();
        }

        public void RequestQuit()
        {
            QuitRequested?.Invoke();
        }
    }
}