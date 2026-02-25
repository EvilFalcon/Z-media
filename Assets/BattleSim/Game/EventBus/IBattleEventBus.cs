using R3;

namespace BattleSim.Game.EventBus
{
    public interface IBattleEventBus
    {
        Observable<int> BattleEnded { get; }
        Observable<(int team0, int team1)> UnitCountChanged { get; }

        void PublishBattleEnded(int winnerTeamId);
        void PublishUnitCount(int team0, int team1);
    }
}

