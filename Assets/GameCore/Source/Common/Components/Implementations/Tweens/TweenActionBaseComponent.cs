using Cysharp.Threading.Tasks;
using Source.Common.Components.Interfaces;
using UnityEngine;

namespace Source.Common.Components.Implementations.Tweens
{
    public abstract class TweenActionBaseComponent : MonoBehaviour, ITweenActionBaseComponent
    {
        public abstract void Initialize();
        public abstract void Cancel();
        public abstract UniTask PlayForward();
        public abstract UniTask PlayBackward();
        public abstract void SetForwardState();
        public abstract void SetBackwardState();
    }
}