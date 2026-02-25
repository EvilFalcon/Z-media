namespace BattleSim.Game.SpawnZone
{
    public sealed class ArmySpawnZones : IArmySpawnZones
    {
        public IArmySpawnZone Left { get; }
        public IArmySpawnZone Right { get; }

        public ArmySpawnZones(IArmySpawnZone left, IArmySpawnZone right)
        {
            Left = left;
            Right = right;
        }
    }
}
