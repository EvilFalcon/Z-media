using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "ColorModifiers", menuName = "BattleSim/Color Modifiers")]
    public sealed class ColorModifiersSO : ScriptableObject
    {
        [Header("Blue")] [SerializeField] private int _blueAtkBonus = -15;
        [SerializeField] private float _blueAtkSpdBonus = 4f;
        [SerializeField] private int _blueSpeedBonus = 10;

        [Header("Green")] [SerializeField] private int _greenHpBonus = -100;
        [SerializeField] private int _greenAtkBonus = 20;
        [SerializeField] private int _greenSpeedBonus = -5;

        [Header("Red")] [SerializeField] private int _redHpBonus = 200;
        [SerializeField] private int _redAtkBonus = 40;
        [SerializeField] private int _redSpeedBonus = -9;

        public void GetModifiers(UnitColorType color, out int hp, out int atk, out int speed, out float atkSpd)
        {
            hp = 0;
            atk = 0;
            speed = 0;
            atkSpd = 0f;

            switch (color)
            {
                case UnitColorType.Blue:
                    atk = _blueAtkBonus;
                    atkSpd = _blueAtkSpdBonus;
                    speed = _blueSpeedBonus;
                    break;
                case UnitColorType.Green:
                    hp = _greenHpBonus;
                    atk = _greenAtkBonus;
                    speed = _greenSpeedBonus;
                    break;
                case UnitColorType.Red:
                    hp = _redHpBonus;
                    atk = _redAtkBonus;
                    speed = _redSpeedBonus;
                    break;
            }
        }
    }
}