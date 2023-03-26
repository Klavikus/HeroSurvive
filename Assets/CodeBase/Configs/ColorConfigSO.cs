using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(menuName = "Create ColorConfigSO", fileName = "ColorConfigSO", order = 0)]
    public class ColorConfigSO : ScriptableObject
    {
        [field: SerializeField] public Color CurrencyColor { get; private set; }
        [field: SerializeField] public Color CurrencyColorCantPay { get; private set; }
        [field: SerializeField] public Color GoodPropertyColor { get; private set; }
        [field: SerializeField] public Color BadPropertyColor { get; private set; }
    }
}