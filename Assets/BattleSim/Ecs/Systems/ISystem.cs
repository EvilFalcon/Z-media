namespace BattleSim.Ecs.Systems
{
    public interface IEcsSystem
    {
        void Run(EcsWorld world, float deltaTime);
    }
}
