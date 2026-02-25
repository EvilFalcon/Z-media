using BattleSim.Ecs;

namespace BattleSim.Game.Strategy
{
    public interface IUnitTargetStrategy
    {
        int SelectTarget(EcsWorld world, int entityId);
    }
}
