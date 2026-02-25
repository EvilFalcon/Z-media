using BattleSim.Game.Core;
using BattleSim.Mvp.GameUI;
using VContainer.Unity;

namespace BattleSim.Bootstrap
{
    public sealed class GameEntryPoint : IStartable
    {
        private readonly GameBootstrap _bootstrap;
        private readonly EcsRunner _runner;
        private readonly GameUIPresenter _uiPresenter;
        
        public GameEntryPoint(GameBootstrap bootstrap, EcsRunner runner, GameUIPresenter uiPresenter)
        {
            _bootstrap = bootstrap;
            _runner = runner;
            _uiPresenter = uiPresenter;
        }

        public void Start()
        {
            _bootstrap.SpawnAndRun();
            _runner.Start();
            _uiPresenter.Initialize();
        }
    }
}
