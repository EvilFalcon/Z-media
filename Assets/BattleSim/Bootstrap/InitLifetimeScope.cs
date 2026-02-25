using BattleSim.Config;
using BattleSim.Game.SceneLoader;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BattleSim.Bootstrap
{
    public sealed class InitLifetimeScope : LifetimeScope
    {
        [SerializeField] private BattleSimSettingsSo _settings;

        protected override void Configure(IContainerBuilder builder)
        {
            //Service
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);

            //Settings
            builder.RegisterInstance(_settings.GameSettings);
            builder.RegisterInstance(_settings.UnitBaseStats);
            builder.RegisterInstance(_settings.FormModifiers);
            builder.RegisterInstance(_settings.SizeModifiers);
            builder.RegisterInstance(_settings.ColorModifiers);
            builder.RegisterInstance(_settings.UnitAppearance);
            builder.RegisterInstance(_settings.UnitPrefabs);
            builder.RegisterInstance(_settings.CombatSettings);

            //Core
            builder.RegisterEntryPoint<InitBootstrap>();
        }
    }
}