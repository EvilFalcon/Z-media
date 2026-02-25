using R3;

namespace BattleSim.Game.EventBus
{
    public sealed class BattleEventBus : IBattleEventBus
    {
        private readonly Subject<int> _battleEnded = new();
        private readonly Subject<(int, int)> _unitCountChanged = new();

        public Observable<int> BattleEnded => _battleEnded;
        public Observable<(int team0, int team1)> UnitCountChanged => _unitCountChanged;

        public void PublishBattleEnded(int winnerTeamId) => _battleEnded.OnNext(winnerTeamId);
        public void PublishUnitCount(int team0, int team1) => _unitCountChanged.OnNext((team0, team1));
    }
}
