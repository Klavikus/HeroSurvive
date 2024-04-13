using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations.Tweens.ConfigsSo
{
    [CreateAssetMenu(menuName = "UIComponents/Create ActionCounterConfig", fileName = "ActionCounterConfig", order = 0)]
    public class ActionCounterConfig : ScriptableObject
    {
        [field: SerializeField] public DelayType DelayType { get; private set; }
        [field: SerializeField] public float CountStep { get; private set; }
        [field: SerializeField] public float CountDuration { get; private set; }
        [field: SerializeField] public string Format { get; private set; }
    }
}