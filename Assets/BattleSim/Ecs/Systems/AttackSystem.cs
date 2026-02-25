using BattleSim.Config;
using BattleSim.Ecs.Components;
using UnityEngine;

namespace BattleSim.Ecs.Systems
{
    public sealed class AttackSystem : IEcsSystem
    {
        private readonly CombatSettingsSO _combatSettings;

        public AttackSystem(CombatSettingsSO combatSettings)
        {
            _combatSettings = combatSettings;
        }

        public void Run(EcsWorld world, float deltaTime)
        {
            var unitPool = world.GetPool<UnitComponent>();
            var positionPool = world.GetPool<PositionComponent>();
            var targetPool = world.GetPool<TargetComponent>();
            var statsPool = world.GetPool<StatsComponent>();
            var boundsPool = world.GetPool<UnitBoundsComponent>();
            var cooldownPool = world.GetPool<AttackCooldownComponent>();

            foreach (var entityId in world.GetAllEntities())
            {
                if (!unitPool.Has(entityId) || !statsPool.Has(entityId) || statsPool.Get(entityId).Hp <= 0)
                    continue;
                if (!boundsPool.Has(entityId))
                    continue;

                if (!targetPool.Has(entityId) || targetPool.Get(entityId).TargetEntityId < 0)
                    continue;

                var targetEntityId = targetPool.Get(entityId).TargetEntityId;
                if (!world.HasEntity(targetEntityId) || 
                    !statsPool.Has(targetEntityId) || 
                    !positionPool.Has(entityId) || 
                    !positionPool.Has(targetEntityId) || 
                    !boundsPool.Has(targetEntityId))
                    continue;

                ref var attackerStats = ref statsPool.Get(entityId);
                ref var targetUnitStats = ref statsPool.Get(targetEntityId);
                if (targetUnitStats.Hp <= 0)
                    continue;

                var attackerRadius = boundsPool.Get(entityId).Radius;
                var targetRadius = boundsPool.Get(targetEntityId).Radius;
                var attackRange = attackerRadius + targetRadius + _combatSettings.AttackGap;
                var distanceToTarget = Vector3.Distance(positionPool.Get(entityId).Value, positionPool.Get(targetEntityId).Value);
                
                if (distanceToTarget > attackRange)
                    continue;

                var attackDelay = 1f / attackerStats.AtkSpd;

                if (!cooldownPool.Has(entityId))
                    cooldownPool.Add(entityId, new AttackCooldownComponent { Remaining = attackDelay });

                ref var cooldownComponent = ref cooldownPool.Get(entityId);
                cooldownComponent.Remaining -= deltaTime;

                if (cooldownComponent.Remaining > 0f)
                    continue;

                cooldownComponent.Remaining = attackDelay;
                targetUnitStats.Hp -= attackerStats.Atk;

                if (targetUnitStats.Hp < 0)
                    targetUnitStats.Hp = 0;
                
                if (!targetPool.Has(targetEntityId))
                    targetPool.Add(targetEntityId, new TargetComponent { TargetEntityId = entityId });
                else
                    targetPool.Get(targetEntityId).TargetEntityId = entityId;
            }
        }
    }
}
