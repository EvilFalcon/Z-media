using BattleSim.Mvp.MainMenu;
using VContainer.Unity;

namespace BattleSim.Bootstrap
{
    public sealed class MainMenuEntryPoint : IStartable
    {
        private readonly MainMenuPresenter _presenter;
        
        public MainMenuEntryPoint(MainMenuPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Start()
        {
            _presenter.Initialize();
        }
    }
}
