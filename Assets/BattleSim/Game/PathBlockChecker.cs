using BattleSim.Config;
using BattleSim.Ecs;
using BattleSim.Ecs.Components;
using UnityEngine;

namespace BattleSim.Game
{
    public sealed class PathBlockChecker : IPathBlockChecker
    {
        private readonly CombatSettingsSO _combatSettings;

        public PathBlockChecker(CombatSettingsSO combatSettings)
        {
            _combatSettings = combatSettings;
        }

        public bool IsPathBlocked(EcsWorld world, int fromEntityId, int toEntityId)
        {
            var positionPool = world.GetPool<PositionComponent>();
            var boundsPool = world.GetPool<UnitBoundsComponent>();
            var statsPool = world.GetPool<StatsComponent>();

            if (!positionPool.Has(fromEntityId) || !boundsPool.Has(fromEntityId) ||
                !positionPool.Has(toEntityId) || !boundsPool.Has(toEntityId))
                return false;

            var myPosition = positionPool.Get(fromEntityId).Value;
            var targetPosition = positionPool.Get(toEntityId).Value;
           
            var myRadius = boundsPool.Get(fromEntityId).Radius;

            var directionToTarget = targetPosition - myPosition;
            var segmentLength = directionToTarget.magnitude;
           
            if (segmentLength < _combatSettings.MinimumDistanceEpsilon) return false;

            foreach (var otherEntityId in world.GetAllEntities())
            {
                if (otherEntityId == fromEntityId || otherEntityId == toEntityId)
                    continue;

                if (!positionPool.Has(otherEntityId) || !boundsPool.Has(otherEntityId) || !statsPool.Has(otherEntityId))
                    continue;

                if (statsPool.Get(otherEntityId).Hp <= 0)
                    continue;

                var otherPosition = positionPool.Get(otherEntityId).Value;
                var otherRadius = boundsPool.Get(otherEntityId).Radius;
                var directionToOther = otherPosition - myPosition;
                var projectionParameter = Vector3.Dot(directionToOther, directionToTarget) / (segmentLength * segmentLength);

                if (projectionParameter < 0f || projectionParameter > 1f)
                    continue;

                var closestPointOnSegment = myPosition + directionToTarget * projectionParameter;
                var distanceToOther = Vector3.Distance(otherPosition, closestPointOnSegment);

                if (distanceToOther < myRadius + otherRadius + _combatSettings.AttackGap)
                    return true;
            }

            return false;
        }
    }
}
