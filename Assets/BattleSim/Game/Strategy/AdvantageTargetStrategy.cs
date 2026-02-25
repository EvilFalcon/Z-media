using System.Linq;
using BattleSim.Config;
using BattleSim.Ecs;
using BattleSim.Ecs.Components;
using UnityEngine;

namespace BattleSim.Game.Strategy
{
    public sealed class AdvantageTargetStrategy : IUnitTargetStrategy
    {
        private readonly CombatSettingsSO _combatSettings;
        private readonly IPathBlockChecker _pathBlockChecker;

        public AdvantageTargetStrategy(CombatSettingsSO combatSettings, IPathBlockChecker pathBlockChecker)
        {
            _combatSettings = combatSettings;
            _pathBlockChecker = pathBlockChecker;
        }

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
            var bestScore = float.MinValue;

            int enemyAlliesNearby = 0;

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

                var enemyPos = posPool.Get(otherId).Value;
                var distSq = (enemyPos - myPos).sqrMagnitude;
                var pathClear = !_pathBlockChecker.IsPathBlocked(world, entityId, otherId);

                var enemyTeam = unitPool.Get(otherId).TeamId;

                enemyAlliesNearby += world
                    .GetAllEntities()
                    .Where(tid => tid != otherId && unitPool.Has(tid) && unitPool.Get(tid).TeamId == enemyTeam)
                    .Where(tid => posPool.Has(tid) && statsPool.Has(tid) && statsPool.Get(tid).Hp > 0)
                    .Count(tid => Vector3.Distance(posPool.Get(tid).Value, enemyPos) <= _combatSettings.IsolationRadius);

                var isolationScore = 1f / (1f + enemyAlliesNearby);
                var distanceScore = 1f / (1f + Mathf.Sqrt(distSq) * _combatSettings.AdvantageDistanceScoreFactor);
                var pathBonus = pathClear ? _combatSettings.AdvantagePathClearBonus : _combatSettings.AdvantagePathBlockedBonus;
                var score = isolationScore * distanceScore * pathBonus;

                if (!(score > bestScore))
                    continue;

                bestScore = score;
                bestTarget = otherId;
            }

            return bestTarget;
        }
    }
}
