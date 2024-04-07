using GameCore.Source.Presentation.Api.GameLoop;
using GameCore.Source.Presentation.Core.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.MainMenu.Upgrades
{
    public class HeroSelectorView : ViewBase, IHeroSelectorView
    {
        [SerializeField] private HeroDescriptionView _heroDescriptionView;
        [SerializeField] private BaseAbilityView _baseAbilityView;

        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public ActionButton ContinueButton { get; private set; }
        [field: SerializeField] public ActionButton CloseButton { get; private set; }
        [field: SerializeField] public RectTransform HeroViewsContainer { get; private set; }
        [field: SerializeField] public RectTransform PropertiesViewContainer { get; private set; }
        [field: SerializeField] public int RowCount { get; private set; }
        [field: SerializeField] public int ColCount { get; private set; }

        public void Initialize()
        {
            ContinueButton.Initialize();
            CloseButton.Initialize();
        }

        public void FillHeroDescription(Sprite icon, string heroName, string description) =>
            _heroDescriptionView.Fill(icon, heroName, description);

        public void FillBaseAbilityView(Sprite icon, string abilityName) =>
            _baseAbilityView.Fill(icon, abilityName);
    }
}