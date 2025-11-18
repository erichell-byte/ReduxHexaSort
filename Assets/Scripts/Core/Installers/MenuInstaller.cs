using Presentation.UI;
using UI.Models;
using UI.Views;
using Zenject;

namespace Core.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MenuModel>().AsSingle();
            Container.Bind<MenuPresenter>().AsSingle();
            Container.Bind<MenuView>().FromComponentInHierarchy().AsSingle();
        }
    }
}
