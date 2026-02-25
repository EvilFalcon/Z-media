using BattleSim.Config;

namespace BattleSim.Game.Strategy
{
    public interface IUnitTargetStrategyFactory
    {
        IUnitTargetStrategy GetStrategy(UnitTactic tactic);
    }
}