using System.Collections.Generic;
using BattleSim.Ecs.Components;
using BattleSim.Game.Repositories;

namespace BattleSim.Ecs.Systems
{
    public sealed class DeathSystem : IEcsSystem
    {
        private readonly List<int> _toRemove = new();
        private readonly IUnitViewRegistry _viewRegistry;

        public DeathSystem(IUnitViewRegistry viewRegistry)
        {
            _viewRegistry = viewRegistry;
        }

        public void Run(EcsWorld world, float deltaTime)
        {
            _toRemove.Clear();
            var statsPool = world.GetPool<StatsComponent>();

            foreach (var entityId in world.GetAllEntities())
            {
                if (!statsPool.Has(entityId))
                    continue;
                if (statsPool.Get(entityId).Hp > 0)
                    continue;

                if (_viewRegistry.TryGetView(entityId, out var view))
                {
                    view.Destroy();
                    _viewRegistry.Unregister(entityId);
                }
                
                _toRemove.Add(entityId);
            }

            foreach (int entityId in _toRemove)
                world.RemoveEntity(entityId);
        }
    }
}
