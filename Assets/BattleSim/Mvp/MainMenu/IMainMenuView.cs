using R3;

namespace BattleSim.Mvp.MainMenu
{
    public interface IMainMenuView
    {
        Observable<Unit> OnStartClicked { get; }
        Observable<Unit> OnRandomizeClicked { get; }
        void SetSeedText(string text);
    }
}
