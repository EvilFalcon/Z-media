using System.Collections.Generic;
using BattleSim.Ecs;
using BattleSim.Ecs.Systems;
using BattleSim.Game.EventBus;
using BattleSim.Game.SceneLoader;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace BattleSim.Game.Core
{
    public sealed class EcsRunner : ITickable
    {
        private readonly EcsWorld _world;
        private readonly List<IEcsSystem> _systems = new();
        private readonly IBattleEventBus _eventBus;
        private readonly ISceneLoader _sceneLoader;
        private readonly CompositeDisposable _disposables = new();
        private bool _running = true;

        public EcsRunner
        (
            EcsWorld world,
            IBattleEventBus eventBus,
            ISceneLoader sceneLoader,
            TargetSelectionSystem targetSelection,
            MovementSystem movement,
            AttackSystem attack,
            DeathSystem death,
            WinCheckSystem winCheck
        )
        {
            _world = world;
            _eventBus = eventBus;
            _sceneLoader = sceneLoader;
            _systems.Add(targetSelection);
            _systems.Add(movement);
            _systems.Add(attack);
            _systems.Add(death);
            _systems.Add(winCheck);
        }

        public void Start()
        {
            _eventBus.BattleEnded
                .Take(1)
                .Subscribe(_ => OnBattleEnded())
                .AddTo(_disposables);
        }

        private void OnBattleEnded()
        {
            _running = false;
            _sceneLoader.ReturnToMainMenu();
        }

        public void Tick()
        {
            if (!_running) return;

            var dt = Time.deltaTime;
            
            foreach (var system in _systems)
                system.Run(_world, dt);
        }
    }
}