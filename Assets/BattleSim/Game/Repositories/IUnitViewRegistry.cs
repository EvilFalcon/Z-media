using BattleSim.Ecs.Components;

namespace BattleSim.Game.Repositories
{
    public interface IUnitViewRegistry
    {
        void Register(int entityId, IUnitView view);
        void Unregister(int entityId);
        bool TryGetView(int entityId, out IUnitView view);
    }
}
