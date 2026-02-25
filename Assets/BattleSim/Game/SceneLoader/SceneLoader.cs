using BattleSim.Config;
using UnityEngine.SceneManagement;

namespace BattleSim.Game.SceneLoader
{
    public sealed class SceneLoader : ISceneLoader
    {
        private readonly GameSettingsSO _settings;

        public SceneLoader(GameSettingsSO settings)
        {
            _settings = settings;
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(_settings.MainMenuSceneName, LoadSceneMode.Additive);
        }
        
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(_settings.MainMenuSceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(_settings.GameSceneName);
        }

        public void LoadGame()
        {
            SceneManager.LoadScene(_settings.GameSceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(_settings.MainMenuSceneName);
        }
    }
}
