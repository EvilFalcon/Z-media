using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "UnitBaseStats", menuName = "BattleSim/Unit Base Stats")]
    public sealed class UnitBaseStatsSO : ScriptableObject
    {
        [SerializeField] private int _baseHp = 100;
        [SerializeField] private int _baseAtk = 10;
        [SerializeField] private int _baseSpeed = 10;
        [SerializeField] private float _baseAtkSpd = 1f;

        public int BaseHp => _baseHp;
        public int BaseAtk => _baseAtk;
        public int BaseSpeed => _baseSpeed;
        public float BaseAtkSpd => _baseAtkSpd;
    }
}