using CodeBase.HeroSelection;
using CodeBase.Infrastructure.Factories;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    [CreateAssetMenu(menuName = "Create ConfigurationContainer", fileName = "ConfigurationContainer", order = 0)]
    public class ConfigurationContainer : ScriptableObject
    {
        [field: SerializeField] public HeroesConfigSO HeroConfigSO { get; private set; }
        [field: SerializeField] public BasePropertiesConfigSO BasePropertiesConfigSO { get; private set; }
        [field: SerializeField] public MainMenuConfiguration MainMenuConfiguration { get; private set; }
    }
}