using BattleSim.Config;
using BattleSim.Ecs.Components;
using BattleSim.Game.SpawnZone;
using UnityEngine;

namespace BattleSim.Presentation
{
    public sealed class UnitViewFactory : IUnitViewFactory
    {
        private readonly UnitPrefabsSO _prefabs;
        private readonly UnitAppearanceSO _appearance;
        private readonly IArmySpawnZones _spawnZones;
        
        public UnitViewFactory(UnitPrefabsSO prefabsSo, UnitAppearanceSO unitAppearanceSo, IArmySpawnZones spawnZones)
        {
            _prefabs = prefabsSo;
            _appearance = unitAppearanceSo;
            _spawnZones = spawnZones;
        }

        public IUnitView Create(UnitFormType form, UnitSizeType size, UnitColorType color, int teamId)
        {
            var zone = teamId == 0 ? _spawnZones.Left : _spawnZones.Right;
            var parent = zone.GetParent();

            var prefab = _prefabs.GetPrefab(form);
            var view = Object.Instantiate(prefab, parent);
            view.SetScale(_appearance.GetScale(size));
            view.SetColor(_appearance.GetColor(color));
           
            return view;
        }
    }
}
