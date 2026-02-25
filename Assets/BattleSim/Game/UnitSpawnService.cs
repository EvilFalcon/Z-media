using System;
using BattleSim.Config;
using BattleSim.Ecs;
using BattleSim.Ecs.Components;
using BattleSim.Game.GroundService;
using BattleSim.Game.Repositories;
using BattleSim.Game.SpawnZone;
using BattleSim.Presentation;
using BattleSim.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleSim.Game
{
    public sealed class UnitSpawnService : IUnitSpawnService
    {
        private readonly IUnitStatsCalculator _calculator;
        private readonly IUnitViewFactory _viewFactory;
        private readonly GameSettingsSO _settings;
        private readonly IArmySpawnZones _spawnZones;
        private readonly IGroundSnap _groundSnap;
        private readonly IUnitViewRegistry _viewRegistry;
        private readonly UnitAppearanceSO _appearance;
        private int _unitColorTypesCount;
        private int _unitSizeTypesCount;
        private int _unitFormTypesCount;

        public UnitSpawnService(
            IUnitStatsCalculator calculator,
            IUnitViewFactory viewFactory,
            GameSettingsSO settings,
            IArmySpawnZones spawnZones,
            IGroundSnap groundSnap,
            IUnitViewRegistry viewRegistry,
            UnitAppearanceSO appearance)
        {
            _calculator = calculator;
            _viewFactory = viewFactory;
            _settings = settings;
            _spawnZones = spawnZones;
            _groundSnap = groundSnap;
            _viewRegistry = viewRegistry;
            _appearance = appearance;
            _unitColorTypesCount = Enum.GetNames(typeof(UnitColorType)).Length;
            _unitSizeTypesCount = Enum.GetNames(typeof(UnitSizeType)).Length;
            _unitFormTypesCount = Enum.GetNames(typeof(UnitFormType)).Length;
        }

        public void SpawnAll(EcsWorld world)
        {
            if (BattleSimRuntimeState.NextBattleSeed >= 0)
            {
                BattleSimRuntimeState.LastUsedSeed = BattleSimRuntimeState.NextBattleSeed;
                Random.InitState(BattleSimRuntimeState.NextBattleSeed);
                BattleSimRuntimeState.NextBattleSeed = -1;
            }
            else
            {
                BattleSimRuntimeState.LastUsedSeed = -1;
            }

            var unitsPerArmyCount = _settings.UnitsPerArmy;
            SpawnArmy(world, 0, unitsPerArmyCount);
            SpawnArmy(world, 1, unitsPerArmyCount);
        }

        private void SpawnArmy(EcsWorld world, int teamId, int count)
        {
            for (var unitIndex = 0; unitIndex < count; unitIndex++)
                SpawnOne(world, teamId, GetSpawnPosition(teamId));
        }

        private Vector3 GetSpawnPosition(int teamId)
        {
            var zone = teamId == 0 ? _spawnZones.Left : _spawnZones.Right;
            if (zone != null)
                return zone.GetRandomPointInside();

            var armyCenterX = teamId == 0 ? _settings.LeftArmyX : _settings.RightArmyX;
            var spawnSpread = _settings.SpawnSpread;

            return new Vector3(armyCenterX, 0f, 0f) + new Vector3(
                Random.Range(-spawnSpread, spawnSpread), 0f, Random.Range(-spawnSpread, spawnSpread));
        }

        private void SpawnOne(EcsWorld world, int teamId, Vector3 position)
        {
            position = _groundSnap.GetPositionOnGround(position);
            var form = Random.Range(0, _unitFormTypesCount).GetValueEnum<UnitFormType>();
            var size = Random.Range(0, _unitSizeTypesCount).GetValueEnum<UnitSizeType>();
            var color = Random.Range(0, _unitColorTypesCount).GetValueEnum<UnitColorType>();
            SpawnOneInternal(world, teamId, position, form, size, color);
        }

        private void SpawnOneInternal(EcsWorld world, int teamId, Vector3 position, UnitFormType form, UnitSizeType size, UnitColorType color)
        {
            _calculator.Compute(form, size, color, out var healthPoints, out var attack, out var speed, out var attackSpeed);

            var entity = world.NewEntity();
            entity.Add(new UnitComponent { Form = form, Size = size, Color = color, TeamId = teamId });
            entity.Add(new StatsComponent { Hp = healthPoints, HpMax = healthPoints, Atk = attack, Speed = speed, AtkSpd = attackSpeed });
            entity.Add(new PositionComponent { Value = position });
            entity.Add(new TargetComponent { TargetEntityId = -1 });
            entity.Add(new AttackCooldownComponent { Remaining = 0f });

            var radius = _appearance.GetScale(size) * 0.5f;
            entity.Add(new UnitBoundsComponent { Radius = radius });
            entity.Add(new UnitStateComponent { State = MovementState.Idle });
            entity.Add(new UnitTacticComponent { Tactic = UnitTactic.Flank });

            var view = _viewFactory.Create(form, size, color, teamId);
            view.SetPosition(position);
            view.SetActive(true);
            _viewRegistry.Register(entity.Id, view);
        }
    }

    public interface IUnitSpawnService
    {
        public void SpawnAll(EcsWorld world);
    }
}