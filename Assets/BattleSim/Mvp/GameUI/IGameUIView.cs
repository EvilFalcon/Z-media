
namespace BattleSim.Mvp.GameUI
{
    public interface IGameUIView
    {
        void SetStatusText(string text);
        void SetUnitCounts(int team0, int team1);
        void SetSeedText(string text);
    }
}
