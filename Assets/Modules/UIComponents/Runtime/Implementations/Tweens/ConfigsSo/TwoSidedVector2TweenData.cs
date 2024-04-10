using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations.Tweens.ConfigsSo
{
    [CreateAssetMenu(menuName = "UIComponents/Create TwoSidedVector2TweenData", fileName = "TwoSidedVector2TweenData",
        order = 0)]
    public class TwoSidedVector2TweenData : ScriptableObject
    {
        [field: SerializeField] public Vector2TweenData Forward { get; private set; }
        [field: SerializeField] public Vector2TweenData Backward { get; private set; }
        [field: SerializeField] public bool IgnoreTimeScale { get; private set; }
    }
}