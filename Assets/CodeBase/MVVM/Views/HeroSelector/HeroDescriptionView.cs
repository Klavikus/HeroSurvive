using CodeBase.Domain.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views.HeroSelector
{
    public class HeroDescriptionView : MonoBehaviour
    {
        [SerializeField] private Image _heroPreview;
        [SerializeField] private TMP_Text _heroName;
        [SerializeField] private TMP_Text _description;

        public void Render(HeroData heroData)
        {
            _heroPreview.sprite = heroData.Sprite;
            _heroName.text = heroData.Name;
            _description.text = heroData.Description;
        }
    }
}