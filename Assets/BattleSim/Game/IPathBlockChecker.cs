using BattleSim.Ecs;

namespace BattleSim.Game
{
    public interface IPathBlockChecker
    {
        bool IsPathBlocked(EcsWorld world, int fromEntityId, int toEntityId);
    }
}