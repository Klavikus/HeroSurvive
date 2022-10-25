using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    [CreateAssetMenu(menuName = "Create MainMenuConfiguration", fileName = "MainMenuConfiguration", order = 0)]
    public class MainMenuConfiguration : ScriptableObject
    {
        public HeroSelectorUI HeroSelector;
    }
}