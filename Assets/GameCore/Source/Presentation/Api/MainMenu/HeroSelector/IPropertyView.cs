using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.MainMenu.HeroSelector
{
    public interface IPropertyView
    {
        TMP_Text Value { get; }
        TMP_Text ShortName { get; }
        Image Icon { get; }
        Transform Transform { get; }
    }
}