using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "BattleSim/Game Settings")]
    public sealed class GameSettingsSO : ScriptableObject
    {
        [SerializeField] private string _mainMenuSceneName = "Main Menu";
        [SerializeField] private string _gameSceneName = "Game";
        [SerializeField] private int _unitsPerArmy = 20;
        [SerializeField] private float _leftArmyX = -10f;
        [SerializeField] private float _rightArmyX = 10f;
        [SerializeField] private float _spawnSpread = 3f;

        [Tooltip("Height of unit pivot above ground so unit stands on surface (e.g. half of unit height).")] [SerializeField]
        private float _unitPivotHeight = 0.5f;

        [Tooltip("Layers for ground raycast (e.g. Terrain). If Nothing, uses DefaultRaycastLayers.")] [SerializeField]
        private LayerMask _groundLayerMask = -1;

        [Tooltip("Layers to exclude from ground raycast (e.g. Default if units use it, so ray does not hit unit colliders).")] [SerializeField]
        private LayerMask _excludeFromGroundRaycast = 0;

        [Tooltip("Radius of unit for separation (units push apart when closer than 2× this). Match collider size.")] [SerializeField]
        private float _unitRadius = 0.5f;

        [Tooltip("Высота начала луча над точкой для поиска земли (GroundSnap).")] [SerializeField]
        private float _rayOriginHeight = 100f;

        [Tooltip("Максимальная дистанция луча вниз при поиске земли (GroundSnap).")] [SerializeField]
        private float _groundRayMaxDistance = 150f;

        public string MainMenuSceneName => _mainMenuSceneName;
        public string GameSceneName => _gameSceneName;
        public int UnitsPerArmy => _unitsPerArmy;
        public float LeftArmyX => _leftArmyX;
        public float RightArmyX => _rightArmyX;
        public float SpawnSpread => _spawnSpread;
        public float UnitPivotHeight => _unitPivotHeight;
        public LayerMask GroundLayerMask => _groundLayerMask;
        public LayerMask ExcludeFromGroundRaycast => _excludeFromGroundRaycast;
        public float UnitRadius => _unitRadius;
        public float RayOriginHeight => _rayOriginHeight;
        public float GroundRayMaxDistance => _groundRayMaxDistance;
    }
}