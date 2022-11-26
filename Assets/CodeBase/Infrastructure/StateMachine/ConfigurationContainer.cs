using CodeBase.Configs;
using CodeBase.ForSort;
using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Views;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    [CreateAssetMenu(menuName = "Create ConfigurationContainer", fileName = "ConfigurationContainer", order = 0)]
    public class ConfigurationContainer : ScriptableObject
    {
        [field: SerializeField] public HeroesConfigSO HeroConfigSO { get; private set; }
        [field: SerializeField] public BasePropertiesConfigSO BasePropertiesConfigSO { get; private set; }
        [field: SerializeField] public MainMenuConfigurationSO MainMenuConfigurationSo { get; private set; }
        [field: SerializeField] public UpgradesConfigSO UpgradesConfigSO { get; private set; }
        [field: SerializeField] public ColorConfigSO ColorConfigSO { get; private set; }
        [field: SerializeField] public GameLoopConfigSO GameLoopConfigSO { get; private set; }
        [field: SerializeField] public EnemyConfigSO EnemyConfigSO { get; private set; }
        [field: SerializeField] public StageCompetitionConfigSO StageCompetitionConfigSO { get; private set; }
        [field: SerializeField] public CoroutineRunner CoroutineRunner { get; private set; }
        [field: SerializeField] public AbilityConfigSO[] AbilityConfigsSO { get; private set; }
        [field: SerializeField] public UserNameView UserNameView { get; private set; }
        [field: SerializeField] public AudioPlayer AudioPlayer { get; private set; }
    }
}