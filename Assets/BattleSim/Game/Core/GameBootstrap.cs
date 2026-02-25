using BattleSim.Ecs;

namespace BattleSim.Game.Core
{
    public sealed class GameBootstrap
    {
        private readonly EcsWorld _world;
        private readonly IUnitSpawnService _spawnService;
        
        public GameBootstrap(EcsWorld world, IUnitSpawnService spawnService)
        {
            _world = world;
            _spawnService = spawnService;
        }

        public void SpawnAndRun()
        {
            _spawnService.SpawnAll(_world);
        }
    }
}
