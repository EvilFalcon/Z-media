using UnityEngine;

namespace BattleSim.Game.GroundService
{
    public interface IGroundSnap
    {
        Vector3 GetPositionOnGround(Vector3 worldPosition);
    }
}
