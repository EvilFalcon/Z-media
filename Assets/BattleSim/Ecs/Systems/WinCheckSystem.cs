using BattleSim.Ecs.Components;
using BattleSim.Game.EventBus;

namespace BattleSim.Ecs.Systems
{
    public sealed class WinCheckSystem : IEcsSystem
    {
        private readonly IBattleEventBus _eventBus;
        private bool _alreadyEnded;

        public WinCheckSystem(IBattleEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Run(EcsWorld world, float deltaTime)
        {
            if (_alreadyEnded)
                return;

            int team0Alive = 0, team1Alive = 0;
            var unitPool = world.GetPool<UnitComponent>();
            var statsPool = world.GetPool<StatsComponent>();

            foreach (var entityId in world.GetAllEntities())
            {
                if (!unitPool.Has(entityId) || !statsPool.Has(entityId))
                    continue;
                
                if (statsPool.Get(entityId).Hp <= 0)
                    continue;

                if (unitPool.Get(entityId).TeamId == 0)
                    team0Alive++;
                
                else team1Alive++;
            }

            _eventBus.PublishUnitCount(team0Alive, team1Alive);

            if (team0Alive != 0 && team1Alive != 0)
                return;

            _alreadyEnded = true;
            var winner = team1Alive == 0 ? 0 : 1;
            _eventBus.PublishBattleEnded(winner);
        }
    }
}
