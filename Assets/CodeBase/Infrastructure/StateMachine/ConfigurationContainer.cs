using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Infrastructure.Services;
using FMODUnity;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    [CreateAssetMenu(menuName = "Create ConfigurationContainer", fileName = "ConfigurationContainer", order = 0)]
    public class ConfigurationContainer : ScriptableObject
    {
        [field: SerializeField] public EventReference FMOD_MainMenuAmbientReference { get; private set; }
        [field: SerializeField] public EventReference FMOD_GameLoopAmbientReference { get; private set; }
        [field: SerializeField] public EventReference FMOD_StartLevelReference { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerDiedReference { get; private set; }
        [field: SerializeField] public EventReference FMOD_UpgradeBuyReference { get; private set; }
        [field: SerializeField] public EventReference FMOD_HitReference { get; private set; }
        [field: SerializeField] public HeroesConfigSO HeroConfigSO { get; private set; }
        [field: SerializeField] public BasePropertiesConfigSO BasePropertiesConfigSO { get; private set; }
        [field: SerializeField] public MainMenuConfigurationSO MainMenuConfigurationSo { get; private set; }
        [field: SerializeField] public UpgradesConfigSO UpgradesConfigSO { get; private set; }
        [field: SerializeField] public ColorConfigSO ColorConfigSO { get; private set; }
        [field: SerializeField] public GameLoopConfigSO GameLoopConfigSO { get; private set; }
        [field: SerializeField] public EnemyConfigSO EnemyConfigSO { get; private set; }
        [field: SerializeField] public StageCompetitionConfigSO StageCompetitionConfigSO { get; private set; }
        [field: SerializeField] public AbilityConfigSO[] AbilityConfigsSO { get; private set; }
        [field: SerializeField] public VfxConfig VfxConfig { get; private set; }
    }
}