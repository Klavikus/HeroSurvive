using UnityEngine;

namespace GameCore.Source.Presentation.Api.MainMenu.HeroSelector
{
    public interface IHeroDescriptionView
    {
        void Fill(Sprite icon, string heroName, string description);
    }
}