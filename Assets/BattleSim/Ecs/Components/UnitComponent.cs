using BattleSim.Config;

namespace BattleSim.Ecs.Components
{
    public struct UnitComponent
    {
        public UnitFormType Form;
        public UnitSizeType Size;
        public UnitColorType Color;
        public int TeamId;
    }
}
