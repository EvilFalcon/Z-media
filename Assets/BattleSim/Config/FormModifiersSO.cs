using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "FormModifiers", menuName = "BattleSim/Form Modifiers")]
    public sealed class FormModifiersSO : ScriptableObject
    {
        [Header("Cube")] [SerializeField] private int _cubeHpBonus = 100;
        [SerializeField] private int _cubeAtkBonus = 10;

        [Header("Sphere")] [SerializeField] private int _sphereHpBonus = 50;
        [SerializeField] private int _sphereAtkBonus = 20;

        public int GetHpBonus(UnitFormType form)
        {
            return form == UnitFormType.Cube ? _cubeHpBonus : _sphereHpBonus;
        }

        public int GetAtkBonus(UnitFormType form)
        {
            return form == UnitFormType.Cube ? _cubeAtkBonus : _sphereAtkBonus;
        }
    }
}