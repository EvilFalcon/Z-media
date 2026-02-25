using BattleSim.Config;
using UnityEngine;

namespace BattleSim.Game.GroundService
{
    public sealed class GroundSnapService : IGroundSnap
    {
        private readonly GameSettingsSO _settings;

        public GroundSnapService(GameSettingsSO settings)
        {
            _settings = settings;
        }

        public Vector3 GetPositionOnGround(Vector3 worldPosition)
        {
            var rayOrigin = worldPosition + Vector3.up * _settings.RayOriginHeight;
            var groundLayerMask = _settings.GroundLayerMask;
            
            if (groundLayerMask == 0) 
                groundLayerMask = Physics.DefaultRaycastLayers;
            
            groundLayerMask &= ~_settings.ExcludeFromGroundRaycast;
            
            if (!Physics.Raycast(rayOrigin, Vector3.down, out var raycastHit, _settings.GroundRayMaxDistance, groundLayerMask))
            {
                var pivotHeightOffset = _settings.UnitPivotHeight;
                
                return new Vector3(worldPosition.x, pivotHeightOffset, worldPosition.z);
            }

            var heightOffset = _settings.UnitPivotHeight;
            
            return new Vector3(worldPosition.x, raycastHit.point.y + heightOffset, worldPosition.z);
        }
    }
}
