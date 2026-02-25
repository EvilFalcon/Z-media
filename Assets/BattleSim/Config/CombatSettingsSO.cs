using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "CombatSettings", menuName = "BattleSim/Combat Settings")]
    public sealed class CombatSettingsSO : ScriptableObject
    {
        [Header("Attack & path")] [Tooltip("Дополнительный зазор к сумме радиусов для дистанции атаки и проверки блокировки пути.")] [SerializeField]
        private float _attackGap = 0.1f;

        [Tooltip("Минимальная дистанция для нормализации направления (избежание деления на ноль).")] [SerializeField]
        private float _minimumDistanceEpsilon = 0.0001f;

        [Header("Flank / waypoint")] [Tooltip("Дистанция смещения точки обхода (waypoint) перпендикулярно направлению на цель.")] [SerializeField]
        private float _flankOffsetDistance = 3f;

        [Tooltip("Радиус достижения waypoint — при входе в него waypoint снимается.")] [SerializeField]
        private float _waypointReachedRadius = 0.6f;

        [Header("Advantage tactic")] [Tooltip("Радиус для подсчёта союзников врага вокруг цели (изолированность цели).")] [SerializeField]
        private float _isolationRadius = 4f;

        [Tooltip("Множитель дистанции в формуле скоринга цели (чем меньше — тем слабее влияние дистанции).")] [SerializeField]
        private float _advantageDistanceScoreFactor = 0.2f;

        [Tooltip("Бонус к скору цели, если путь до неё свободен.")] [SerializeField]
        private float _advantagePathClearBonus = 1f;

        [Tooltip("Бонус к скору цели, если путь заблокирован (меньше чем path clear).")] [SerializeField]
        private float _advantagePathBlockedBonus = 0.3f;

        public float AttackGap => _attackGap;
        public float MinimumDistanceEpsilon => _minimumDistanceEpsilon;
        public float FlankOffsetDistance => _flankOffsetDistance;
        public float WaypointReachedRadius => _waypointReachedRadius;
        public float IsolationRadius => _isolationRadius;
        public float AdvantageDistanceScoreFactor => _advantageDistanceScoreFactor;
        public float AdvantagePathClearBonus => _advantagePathClearBonus;
        public float AdvantagePathBlockedBonus => _advantagePathBlockedBonus;
    }
}