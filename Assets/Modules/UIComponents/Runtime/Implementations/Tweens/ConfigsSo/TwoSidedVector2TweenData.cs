using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations.Tweens
{
    [CreateAssetMenu(menuName = "UIComponents/Create TwoSidedVector2TweenData", fileName = "TwoSidedVector2TweenData",
        order = 0)]
    public class TwoSidedVector2TweenData : ScriptableObject
    {
        [field: SerializeField] public Vector2TweenData Forward { get; set; }
        [field: SerializeField] public Vector2TweenData Backward { get; set; }
    }
}