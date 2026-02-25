using System.Runtime.CompilerServices;
using UnityEngine;

namespace BattleSim.Game.PlayAreaBounds
{
    public sealed class PlayAreaBoundsFromTerrain : IPlayAreaBounds
    {
        private readonly float _minX;
        private readonly float _maxX;
        private readonly float _minZ;
        private readonly float _maxZ;

        public PlayAreaBoundsFromTerrain(Terrain terrain)
        {
            if (terrain == null || terrain.terrainData == null)
            {
                _minX = _minZ = float.MinValue;
                _maxX = _maxZ = float.MaxValue;
                return;
            }

            var data = terrain.terrainData;
            var transform = terrain.transform;
            
            var halfWidth = data.size.x * 0.5f;
            var halfLength = data.size.z * 0.5f;
            
            var center = transform.position + new Vector3(halfWidth, 0, halfLength);
            
            var widthInWorld = data.size.x * transform.lossyScale.x;
            var lengthInWorld = data.size.z * transform.lossyScale.z;
            
            _minX = center.x - widthInWorld * 0.5f;
            _maxX = center.x + widthInWorld * 0.5f;
            _minZ = center.z - lengthInWorld * 0.5f;
            _maxZ = center.z + lengthInWorld * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClampPosition(ref Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _minX, _maxX);
            position.z = Mathf.Clamp(position.z, _minZ, _maxZ);
        }
    }
}
