using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TweenTrigger : MonoBehaviour, ITweenTrigger, IPointerEnterHandler, IPointerExitHandler
{
    public event Action Showed;
    public event Action Hided;

    public void InvokeShow() => Showed?.Invoke();
    public void InvokeHide() => Hided?.Invoke();
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        InvokeShow();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InvokeHide();
    }
}