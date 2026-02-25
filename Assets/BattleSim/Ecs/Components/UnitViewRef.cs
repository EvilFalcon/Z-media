using UnityEngine;

namespace BattleSim.Ecs.Components
{
    public interface IUnitView
    {
        void SetPosition(Vector3 position);
        void SetActive(bool active);
        void Destroy();
    }
}
