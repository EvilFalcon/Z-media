using UnityEngine;

namespace BattleSim.Game.SpawnZone
{
    public sealed class ArmySpawnZone : MonoBehaviour, IArmySpawnZone
    {
        [SerializeField] private Collider _zoneCollider;
        
        public Transform GetParent() => transform;

        public Vector3 GetRandomPointInside()
        {
            if (_zoneCollider == null)
                return transform.position;

            var b = _zoneCollider.bounds;
            return new Vector3(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y),
                Random.Range(b.min.z, b.max.z));
        }
    }
}
