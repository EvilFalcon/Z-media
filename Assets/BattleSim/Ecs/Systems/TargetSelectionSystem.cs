using BattleSim.Config;
using BattleSim.Ecs.Components;
using BattleSim.Game.Strategy;

namespace BattleSim.Ecs.Systems
{
    public sealed class TargetSelectionSystem : IEcsSystem
    {
        private readonly IUnitTargetStrategyFactory _strategyFactory;

        public TargetSelectionSystem(IUnitTargetStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public void Run(EcsWorld world, float deltaTime)
        {
            var unitPool = world.GetPool<UnitComponent>();
            var posPool = world.GetPool<PositionComponent>();
            var targetPool = world.GetPool<TargetComponent>();
            var statsPool = world.GetPool<StatsComponent>();
            var tacticPool = world.GetPool<UnitTacticComponent>();

            foreach (var entityId in world.GetAllEntities())
            {
                if (!unitPool.Has(entityId) || !statsPool.Has(entityId) || !posPool.Has(entityId))
                    continue;

                if (!targetPool.Has(entityId))
                    targetPool.Add(entityId, new TargetComponent { TargetEntityId = -1 });

                ref var target = ref targetPool.Get(entityId);

                if (target.TargetEntityId >= 0 && world.HasEntity(target.TargetEntityId) && statsPool.Has(target.TargetEntityId))
                {
                    ref var targetStats = ref statsPool.Get(target.TargetEntityId);
                    
                    if (targetStats.Hp > 0)
                        continue;
                }

                var tactic = tacticPool.Has(entityId) ? tacticPool.Get(entityId).Tactic : UnitTactic.Flank;
                var strategy = _strategyFactory.GetStrategy(tactic);
                target.TargetEntityId = strategy.SelectTarget(world, entityId);
            }
        }
    }
}
