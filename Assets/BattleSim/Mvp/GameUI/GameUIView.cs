using TMPro;
using UnityEngine;

namespace BattleSim.Mvp.GameUI
{
    public sealed class GameUIView : MonoBehaviour, IGameUIView
    {
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private TextMeshProUGUI _unitCountText;
        [SerializeField] private TextMeshProUGUI _seedText;

        public void SetStatusText(string text)
        {
            _statusText.text = text;
        }

        public void SetUnitCounts(int team0, int team1)
        {
            _unitCountText.text = $"Team 1: {team0}  |  Team 2: {team1}"; //TODO There should be a localization service in production
        }

        public void SetSeedText(string text)
        {
            _seedText.text = text;
        }
    }
}