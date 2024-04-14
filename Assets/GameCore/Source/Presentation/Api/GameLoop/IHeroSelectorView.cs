using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IHeroSelectorView
    {
        int RowCount { get; }
        int ColCount { get; }
        ActionButton ContinueButton { get; }
        ActionButton CloseButton { get; }
        RectTransform HeroViewsContainer { get; }
        RectTransform PropertiesViewContainer { get; }
        void Initialize();
        void Show();
        void Hide();
        void FillHeroDescription(Sprite icon, string heroName, string description);
        void FillBaseAbilityView(Sprite icon, string abilityName);
    }
}