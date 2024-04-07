using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IPersistentUpgradeView
    {
        TMP_Text Name { get; }
        Image Icon { get; }
        Image SelectionBorder { get; }
        RectTransform UpgradeLevelViewsContainer { get; }
        Transform Transform { get; }
        event Action Clicked;
    }
}