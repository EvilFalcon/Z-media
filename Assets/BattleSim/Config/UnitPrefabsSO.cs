using UnityEngine;
using BattleSim.Presentation;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "UnitPrefabs", menuName = "BattleSim/Unit Prefabs")]
    public sealed class UnitPrefabsSO : ScriptableObject
    {
        [SerializeField] private UnitView _cubePrefab;
        [SerializeField] private UnitView _spherePrefab;

         //TODO In production, references to models must be stored in addressables. This prevents storing unnecessary objects in memory.

        public UnitView GetPrefab(UnitFormType form)
        {
            return form == UnitFormType.Cube ? _cubePrefab : _spherePrefab;
        }
    }
}
