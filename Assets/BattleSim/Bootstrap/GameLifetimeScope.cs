using UnityEngine;
using VContainer;
using VContainer.Unity;
using BattleSim.Ecs;
using BattleSim.Ecs.Systems;
using BattleSim.Mvp.GameUI;
using BattleSim.Presentation;
using BattleSim.Config;
using BattleSim.Game;
using BattleSim.Game.Core;
using BattleSim.Game.EventBus;
using BattleSim.Game.GroundService;
using BattleSim.Game.PlayAreaBounds;
using BattleSim.Game.Repositories;
using BattleSim.Game.SpawnZone;
using BattleSim.Game.Strategy;

namespace BattleSim.Bootstrap
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameUIView _gameUIView;
        [SerializeField] private ArmySpawnZone _leftSpawnZone;
        [SerializeField] private ArmySpawnZone _rightSpawnZone;
        [SerializeField] private Terrain _terrain;

        protected override void Configure(IContainerBuilder builder)
        {
            var spawnZones = new ArmySpawnZones(_leftSpawnZone, _rightSpawnZone);
            builder.RegisterInstance<IArmySpawnZones>(spawnZones);
            builder.RegisterInstance(_terrain);

            builder.Register<IPathBlockChecker, PathBlockChecker>(Lifetime.Singleton);
            builder.Register<NearestTargetStrategy>(Lifetime.Singleton);
            builder.Register<FlankTargetStrategy>(Lifetime.Singleton);
            builder.Register<AdvantageTargetStrategy>(Lifetime.Singleton);
            builder.Register<IUnitTargetStrategyFactory, UnitTargetStrategyFactory>(Lifetime.Singleton);

            //ECS
            builder.Register<EcsWorld>(Lifetime.Singleton);
            builder.Register<TargetSelectionSystem>(Lifetime.Singleton);
            builder.Register<MovementSystem>(Lifetime.Singleton);
            builder.Register<AttackSystem>(Lifetime.Singleton);
            builder.Register<DeathSystem>(Lifetime.Singleton);
            builder.Register<WinCheckSystem>(Lifetime.Singleton);
            builder.Register<EcsRunner>(Lifetime.Singleton);

            //Service
            builder.Register<IGroundSnap, GroundSnapService>(Lifetime.Singleton);
            builder.Register<IUnitViewRegistry, UnitViewRegistry>(Lifetime.Singleton);
            builder.Register<IUnitSpawnService, UnitSpawnService>(Lifetime.Singleton);
            builder.Register<IBattleEventBus, BattleEventBus>(Lifetime.Singleton);
            builder.Register<IPlayAreaBounds, PlayAreaBoundsFromTerrain>(Lifetime.Singleton);

            //Utils
            builder.Register<IUnitStatsCalculator, UnitStatsCalculator>(Lifetime.Singleton);

            //Views
            builder.RegisterInstance<IGameUIView>(_gameUIView);
            builder.Register<GameUIPresenter>(Lifetime.Singleton);

            //EntryPoint
            builder.RegisterEntryPoint<GameEntryPoint>();
            builder.RegisterEntryPoint<EcsRunner>();
            builder.Register<GameBootstrap>(Lifetime.Singleton);

            //Factories
            builder.Register<IUnitViewFactory, UnitViewFactory>(Lifetime.Singleton);
        }
    }
}