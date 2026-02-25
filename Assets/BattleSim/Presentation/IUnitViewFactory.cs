using BattleSim.Config;
using BattleSim.Ecs.Components;

namespace BattleSim.Presentation
{
    public interface IUnitViewFactory
    {
        IUnitView Create(UnitFormType form, UnitSizeType size, UnitColorType color, int teamId);
    }
}
