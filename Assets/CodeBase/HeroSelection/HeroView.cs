using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _selectionBorder;
    [SerializeField] private Image _heroImage;

    public event Action<HeroView> Clicked;

    public void Initialize(Sprite image)
    {
        _heroImage.sprite = image;
        Deselect();
    }
    public void Select() => _selectionBorder.enabled = true;
    
    public void Deselect() => _selectionBorder.enabled = false;

    public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(this);
}