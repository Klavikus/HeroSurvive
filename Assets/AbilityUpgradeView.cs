using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUpgradeView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _description;

    public event Action<AbilityUpgradeView> Selected;

    public void OnPointerClick(PointerEventData eventData) => Selected?.Invoke(this);

    public void Show(AbilityUpgradeViewData abilityUpgradeViewData)
    {
        gameObject.SetActive(true);
        _icon.sprite = abilityUpgradeViewData.Icon;
        _description.text = abilityUpgradeViewData.Description;
    }

    public void Hide() => gameObject.SetActive(false);
}