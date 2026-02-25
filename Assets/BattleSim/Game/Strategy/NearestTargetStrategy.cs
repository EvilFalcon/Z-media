using BattleSim.Ecs;
using BattleSim.Ecs.Components;

namespace BattleSim.Game.Strategy
{
    public sealed class NearestTargetStrategy : IUnitTargetStrategy
    {
        public int SelectTarget(EcsWorld world, int entityId)
        {
            var unitPool = world.GetPool<UnitComponent>();
            var posPool = world.GetPool<PositionComponent>();
            var statsPool = world.GetPool<StatsComponent>();

            if (!unitPool.Has(entityId) || !posPool.Has(entityId) || !statsPool.Has(entityId))
                return -1;

            var myPos = posPool.Get(entityId).Value;
            var myTeam = unitPool.Get(entityId).TeamId;

            var bestTarget = -1;
            var bestDistSq = float.MaxValue;

            foreach (var otherId in world.GetAllEntities())
            {
                if (otherId == entityId) 
                    continue;
                
                if (!unitPool.Has(otherId) || unitPool.Get(otherId).TeamId == myTeam)
                    continue;
                
                if (!statsPool.Has(otherId) || statsPool.Get(otherId).Hp <= 0)
                    continue;
                
                if (!posPool.Has(otherId)) 
                    continue;

                var distSq = (posPool.Get(otherId).Value - myPos).sqrMagnitude;
                
                if (distSq >= bestDistSq)
                    continue;

                bestDistSq = distSq;
                bestTarget = otherId;
            }

            return bestTarget;
        }
    }
}
