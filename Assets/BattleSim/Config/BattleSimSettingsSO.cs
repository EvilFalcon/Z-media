using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "BattleSimSettings", menuName = "BattleSim/Settings Root")]
    public sealed class BattleSimSettingsSo : ScriptableObject
    {
        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private UnitBaseStatsSO _unitBaseStats;
        [SerializeField] private FormModifiersSO _formModifiers;
        [SerializeField] private SizeModifiersSO _sizeModifiers;
        [SerializeField] private ColorModifiersSO _colorModifiers;
        [SerializeField] private UnitAppearanceSO _unitAppearance;
        [SerializeField] private UnitPrefabsSO _unitPrefabs;
        [SerializeField] private CombatSettingsSO _combatSettings;

        public GameSettingsSO GameSettings => _gameSettings;
        public UnitBaseStatsSO UnitBaseStats => _unitBaseStats;
        public FormModifiersSO FormModifiers => _formModifiers;
        public SizeModifiersSO SizeModifiers => _sizeModifiers;
        public ColorModifiersSO ColorModifiers => _colorModifiers;
        public UnitAppearanceSO UnitAppearance => _unitAppearance;
        public UnitPrefabsSO UnitPrefabs => _unitPrefabs;
        public CombatSettingsSO CombatSettings => _combatSettings;
    }
}
