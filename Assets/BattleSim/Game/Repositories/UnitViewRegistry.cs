using System.Collections.Generic;
using BattleSim.Ecs.Components;

namespace BattleSim.Game.Repositories
{
    public sealed class UnitViewRegistry : IUnitViewRegistry
    {
        private readonly Dictionary<int, IUnitView> _entityIdToView = new();

        public void Register(int entityId, IUnitView view)
        {
            _entityIdToView[entityId] = view;
        }

        public void Unregister(int entityId)
        {
            _entityIdToView.Remove(entityId);
        }

        public bool TryGetView(int entityId, out IUnitView view)
        {
            return _entityIdToView.TryGetValue(entityId, out view);
        }
    }
}
