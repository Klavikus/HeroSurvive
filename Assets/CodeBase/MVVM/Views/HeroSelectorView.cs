using UnityEngine;

namespace CodeBase.MVVM.Views
{
    public class HeroSelectorView : MonoBehaviour
    {
        [SerializeField] private RectTransform _heroViewsContainer;
        [SerializeField] private RectTransform _propertiesViewContainer;
        [SerializeField] private RectTransform _descriptionViewContainer;
        [SerializeField] private RectTransform _baseAbilityViewContainer;

        public void SetHeroViews(HeroView[] heroViews)
        {
            foreach (HeroView heroView in heroViews)
            {
                heroView.transform.SetParent(_heroViewsContainer);
                heroView.transform.localScale = Vector3.one;
            }
        }

        public void SetDescriptionView(HeroDescriptionView descriptionView)
        {
            RectTransform rectTransform = descriptionView.GetComponent<RectTransform>();
            rectTransform.SetParent(_descriptionViewContainer);
            rectTransform.localScale = Vector3.one;
            rectTransform.offsetMax = Vector2.one;
            rectTransform.offsetMin = Vector2.one;
        }
        
        public void SetPropertyViews(PropertyView[] propertiesViews)
        {
            foreach (PropertyView propertyView in propertiesViews)
            {
                propertyView.transform.SetParent(_propertiesViewContainer);
                propertyView.transform.localScale = Vector3.one;
            }
        }

        public void SetBaseAbilityView(BaseAbilityView baseAbilityView)
        {
            RectTransform rectTransform = baseAbilityView.GetComponent<RectTransform>();
            rectTransform.SetParent(_baseAbilityViewContainer);
            rectTransform.localScale = Vector3.one;
            rectTransform.offsetMax = Vector2.one;
            rectTransform.offsetMin = Vector2.one;
        }
    }
}