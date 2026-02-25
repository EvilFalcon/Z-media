using BattleSim.Ecs;
using BattleSim.Ecs.Components;

namespace BattleSim.Game.Strategy
{
    public sealed class FlankTargetStrategy : IUnitTargetStrategy
    {
        private readonly IPathBlockChecker _pathBlockChecker;

        public FlankTargetStrategy(IPathBlockChecker pathBlockChecker)
        {
            _pathBlockChecker = pathBlockChecker;
        }

        public int SelectTarget(EcsWorld world, int entityId)
        {
            var unitPool = world.GetPool<UnitComponent>();
            var positionPool = world.GetPool<PositionComponent>();
            var statsPool = world.GetPool<StatsComponent>();

            if (!unitPool.Has(entityId) || !positionPool.Has(entityId) || !statsPool.Has(entityId))
                return -1;

            var myPosition = positionPool.Get(entityId).Value;
            var myTeam = unitPool.Get(entityId).TeamId;

            var bestUnblocked = -1;
            var bestUnblockedDistSq = float.MaxValue;
            var bestBlocked = -1;
            var bestBlockedDistSq = float.MaxValue;

            foreach (var otherId in world.GetAllEntities())
            {
                if (otherId == entityId)
                    continue;
                
                if (!unitPool.Has(otherId) || unitPool.Get(otherId).TeamId == myTeam)
                    continue;
                
                if (!statsPool.Has(otherId) || statsPool.Get(otherId).Hp <= 0)
                    continue;
                
                if (!positionPool.Has(otherId)) 
                    continue;

                var distSq = (positionPool.Get(otherId).Value - myPosition).sqrMagnitude;
                var blocked = _pathBlockChecker.IsPathBlocked(world, entityId, otherId);

                if (blocked)
                {
                    if (!(distSq < bestBlockedDistSq))
                        continue;

                    bestBlockedDistSq = distSq;
                    bestBlocked = otherId;
                }
                else
                {
                    if (!(distSq < bestUnblockedDistSq))
                        continue;

                    bestUnblockedDistSq = distSq;
                    bestUnblocked = otherId;
                }
            }

            return bestUnblocked >= 0 ? bestUnblocked : bestBlocked;
        }
    }
}
