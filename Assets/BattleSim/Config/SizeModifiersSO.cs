using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "SizeModifiers", menuName = "BattleSim/Size Modifiers")]
    public sealed class SizeModifiersSO : ScriptableObject
    {
        [Header("Big")] [SerializeField] private int _bigHpBonus = 50;

        [Header("Small")] [SerializeField] private int _smallHpBonus = -50;

        public int GetHpBonus(UnitSizeType size)
        {
            return size == UnitSizeType.Big ? _bigHpBonus : _smallHpBonus;
        }
    }
}