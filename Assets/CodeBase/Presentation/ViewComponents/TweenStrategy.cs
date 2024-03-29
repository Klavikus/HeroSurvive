using UnityEngine;

namespace CodeBase.Presentation.ViewComponents
{
    [CreateAssetMenu(menuName = "Create TweenStrategy", fileName = "TweenStrategy", order = 0)]
    public class TweenStrategy : ScriptableObject
    {
        [field: SerializeField] public float ScaleUpModifier { get; private set; }
        [field: SerializeField] public float LoopDuration { get; private set; }
    }
}