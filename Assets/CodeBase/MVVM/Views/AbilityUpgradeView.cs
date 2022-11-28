using System;
using CodeBase.MVVM.Builders;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUpgradeView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _selectionBorder;
    [SerializeField] private Image _selectionBackground;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _title;

    private UpgradeDescriptionBuilder _upgradeDescriptionBuilder;
    public event Action<AbilityUpgradeView> Selected;

    public void Initialize(UpgradeDescriptionBuilder upgradeDescriptionBuilder) =>
        _upgradeDescriptionBuilder = upgradeDescriptionBuilder;

    public void OnPointerClick(PointerEventData eventData) => Selected?.Invoke(this);

    public void Show(AbilityUpgradeData abilityUpgradeData)
    {
        gameObject.SetActive(true);
        //TODO: Доделать билдер для иконок и названия, с учётом локализации
        _icon.sprite = abilityUpgradeData.BaseConfigSO.UpgradeViewData.Icon;
        _title.text = abilityUpgradeData.BaseConfigSO.UpgradeViewData.Name;
        _description.text = _upgradeDescriptionBuilder.GetAbilityUpgradeDescription(abilityUpgradeData);
        _selectionBorder.enabled = false;
        _selectionBackground.enabled = false;
    }

    public void Hide() => gameObject.SetActive(false);

    public void SetSelected(bool isSelected)
    {
        _selectionBorder.enabled = isSelected;
        _selectionBackground.enabled = isSelected;
    }
}