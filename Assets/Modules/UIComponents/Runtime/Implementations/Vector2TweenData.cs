using System;
using DG.Tweening;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations
{
    [Serializable]
    public struct Vector2TweenData
    {
        public Vector2 Value;
        public float Duration;
        public Ease Ease;
    }
}