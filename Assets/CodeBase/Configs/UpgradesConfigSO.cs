using CodeBase.Domain.Data;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(menuName = "Create UpgradesConfigSO", fileName = "UpgradesConfigSO", order = 0)]
    public class UpgradesConfigSO : ScriptableObject
    {
        [field: SerializeField] public UpgradeData[] UpgradeData { get; private set; }
    }
}