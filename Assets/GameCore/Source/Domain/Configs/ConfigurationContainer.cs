using UnityEngine;

namespace Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create ConfigurationContainer", fileName = "ConfigurationContainer", order = 0)]
    public class ConfigurationContainer : ScriptableObject
    {
        [field: SerializeField] public string InitialSceneName { get; private set; }
        [field: SerializeField] public string DataSaveKey { get; private set; }
        [field: SerializeField] public string MainMenuSceneName { get; private set; }
        [field: SerializeField] public LevelViewSo LevelViewSo { get; private set; }
    }
}