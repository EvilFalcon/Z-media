using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSim.Mvp.MainMenu
{
    public sealed class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _randomizeButton;
        [SerializeField] private TextMeshProUGUI _seedText;

        private readonly Subject<Unit> _onStartClicked = new();
        private readonly Subject<Unit> _onRandomizeClicked = new();

        public Observable<Unit> OnStartClicked => _onStartClicked;
        public Observable<Unit> OnRandomizeClicked => _onRandomizeClicked;

        private void Awake()
        {
            if (_startButton != null)
                _startButton.onClick.AddListener(() => _onStartClicked.OnNext(Unit.Default));
            
            if (_randomizeButton != null)
                _randomizeButton.onClick.AddListener(() => _onRandomizeClicked.OnNext(Unit.Default));
        }

        public void SetSeedText(string text)
        {
            if (_seedText != null)
                _seedText.text = text;
        }

        private void OnDestroy()
        {
            _onStartClicked.Dispose();
            _onRandomizeClicked.Dispose();
        }
    }
}
