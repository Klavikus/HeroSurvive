using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.MainMenu.HeroSelector
{
    public interface ISelectableHeroView
    {
        Image SelectionBorder { get; }
        Image Image { get; }
        Transform Transform { get; }
        event Action Clicked;
    }
}