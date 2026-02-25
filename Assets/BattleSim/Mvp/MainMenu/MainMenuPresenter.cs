using BattleSim.Game;
using BattleSim.Game.SceneLoader;
using UnityEngine;
using R3;
using VContainer;

namespace BattleSim.Mvp.MainMenu
{
    public sealed class MainMenuPresenter
    {
        private readonly IMainMenuView _view;
        private readonly ISceneLoader _sceneLoader;
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        public MainMenuPresenter(IMainMenuView view, ISceneLoader sceneLoader)
        {
            _view = view;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            //TODO This is a purely technical implementation.
            //In production, all text must pass through the localization service before being installed in the view.
            //Also, all text must be stored in SO.
            
            _view.OnStartClicked
                .Subscribe(_ => _sceneLoader.LoadGame())
                .AddTo(_disposables);

            _view.OnRandomizeClicked
                .Subscribe(_ =>
                {
                    BattleSimRuntimeState.NextBattleSeed = Random.Range(0, int.MaxValue);
                    _view.SetSeedText($"Seed: {BattleSimRuntimeState.NextBattleSeed}");
                })
                .AddTo(_disposables);

            _view.SetSeedText(BattleSimRuntimeState.NextBattleSeed >= 0
                ? $"Seed: {BattleSimRuntimeState.NextBattleSeed}"
                : "Seed: (press Randomize)");
        }
    }
}
