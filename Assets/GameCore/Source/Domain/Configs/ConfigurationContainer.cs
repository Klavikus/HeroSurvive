using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create ConfigurationContainer", fileName = "ConfigurationContainer", order = 0)]
    public class ConfigurationContainer : ScriptableObject
    {
        [field: SerializeField] public HeroesConfigSO HeroConfigSO { get; private set; }
        [field: SerializeField] public BasePropertiesConfigSO BasePropertiesConfigSO { get; private set; }
        [field: SerializeField] public UpgradesConfigSO UpgradesConfigSO { get; private set; }
        [field: SerializeField] public ColorConfigSO ColorConfigSO { get; private set; }
        [field: SerializeField] public GameLoopConfigSO GameLoopConfigSO { get; private set; }
        [field: SerializeField] public EnemyConfigSO EnemyConfigSO { get; private set; }
        [field: SerializeField] public StageCompetitionConfigSO StageCompetitionConfigSO { get; private set; }
        [field: SerializeField] public AbilityConfigSO[] AbilityConfigsSO { get; private set; }
        [field: SerializeField] public VfxConfig VfxConfig { get; private set; }
        [field: SerializeField] public string LocalizationTablePath { get; private set; }
        [field: SerializeField] public string BaseLanguage { get; private set; }
    }
}