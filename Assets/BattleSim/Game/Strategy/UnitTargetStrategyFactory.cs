using BattleSim.Config;

namespace BattleSim.Game.Strategy
{
    public sealed class UnitTargetStrategyFactory : IUnitTargetStrategyFactory
    {
        private readonly NearestTargetStrategy _nearest;
        private readonly FlankTargetStrategy _flank;
        private readonly AdvantageTargetStrategy _advantage;

        public UnitTargetStrategyFactory(
            NearestTargetStrategy nearest,
            FlankTargetStrategy flank,
            AdvantageTargetStrategy advantage)
        {
            _nearest = nearest;
            _flank = flank;
            _advantage = advantage;
        }

        public IUnitTargetStrategy GetStrategy(UnitTactic tactic)
        {
            switch (tactic)
            {
                case UnitTactic.Direct: 
                    return _nearest;
                case UnitTactic.Flank: 
                    return _flank;
                case UnitTactic.TakeAdvantage:
                    return _advantage;
                default: 
                    return _flank;
            }
        }
    }
}
