using UnityEngine;

namespace BattleSim.Game.SpawnZone
{
    public interface IArmySpawnZone
    {
        Transform GetParent();
        Vector3 GetRandomPointInside();
    }
}
