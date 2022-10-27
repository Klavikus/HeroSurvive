using CodeBase.MVVM.Views;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    [CreateAssetMenu(menuName = "Create MainMenuConfiguration", fileName = "MainMenuConfiguration", order = 0)]
    public class MainMenuConfiguration : ScriptableObject
    {
        public HeroSelectorView HeroSelectorView;
        public HeroDescriptionView HeroDescriptionView;
        public BaseAbilityView BaseAbilityView;
    }
}