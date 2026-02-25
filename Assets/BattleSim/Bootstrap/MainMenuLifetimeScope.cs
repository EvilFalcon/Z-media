using UnityEngine;
using VContainer;
using VContainer.Unity;
using BattleSim.Mvp.MainMenu;

namespace BattleSim.Bootstrap
{
    public sealed class MainMenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainMenuView _mainMenuView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<IMainMenuView>(_mainMenuView);

            builder.Register<MainMenuPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MainMenuEntryPoint>();
        }
    }
}