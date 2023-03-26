using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Presentation
{
    public class TweenTrigger : MonoBehaviour, ITweenTrigger, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ITweenTrigger> Showed;
        public event Action<ITweenTrigger> Hided;

        public void InvokeShow() => Showed?.Invoke(this);
        public void InvokeHide() => Hided?.Invoke(this);

        public void OnPointerEnter(PointerEventData eventData) => InvokeShow();
        public void OnPointerExit(PointerEventData eventData) => InvokeHide();
    }
}