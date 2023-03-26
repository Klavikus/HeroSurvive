using CodeBase.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Presentation
{
    public class HeroView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _selectionBorder;
        [SerializeField] private Image _heroImage;

        private HeroSelectorViewModel _heroSelectorViewModel;
        private HeroData _data;

        private void OnEnable()
        {
            if (_heroSelectorViewModel == null)
                return;

            _heroSelectorViewModel.HeroSelected += OnHeroSelected;
        }

        private void OnDisable()
        {
            if (_heroSelectorViewModel == null)
                return;

            _heroSelectorViewModel.HeroSelected -= OnHeroSelected;
        }

        public void Initialize(HeroData data, HeroSelectorViewModel heroSelectorViewModel)
        {
            _data = data;
            _heroSelectorViewModel = heroSelectorViewModel;
            _heroImage.sprite = _data.Sprite;
            _selectionBorder.enabled = _heroSelectorViewModel.CurrentSelectedHeroData == data;
            _heroSelectorViewModel.HeroSelected += OnHeroSelected;
        }

        private void OnHeroSelected(HeroData selectedHeroData) => _selectionBorder.enabled = _data == selectedHeroData;

        public void OnPointerClick(PointerEventData eventData) => _heroSelectorViewModel.SelectHero(_data);
    }
}