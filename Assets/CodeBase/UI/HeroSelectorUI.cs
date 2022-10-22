using System;
using CodeBase.HeroSelection;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class HeroSelectorUI : MonoBehaviour
    {
        [SerializeField] private GameObject _mainPanel;

        public Action<Hero> HeroSelected;
        
        public void Show()
        {
            _mainPanel.SetActive(true);
        }
    }
}