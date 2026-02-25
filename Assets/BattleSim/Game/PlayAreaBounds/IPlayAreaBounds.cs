using UnityEngine;

namespace BattleSim.Game.PlayAreaBounds
{
    public interface IPlayAreaBounds
    {
        void ClampPosition(ref Vector3 position);
    }
}
