using BattleSim.Game.SceneLoader;
using VContainer.Unity;

namespace BattleSim.Bootstrap
{
    public sealed class InitBootstrap : IStartable
    {
        private readonly ISceneLoader _sceneLoader;
        
        public InitBootstrap(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Start()
        {
            _sceneLoader.LoadMainMenu();
        }
    }
}
