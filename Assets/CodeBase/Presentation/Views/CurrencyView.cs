using CodeBase.Infrastructure.Builders;
using CodeBase.Presentation.ViewModels;
using TMPro;
using UnityEngine;

namespace CodeBase.Presentation.Views
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amount;

        private CurrencyViewModel _currencyViewModel;
        private UpgradeDescriptionBuilder _descriptionBuilder;

        public void Initialize(CurrencyViewModel currencyViewModel, UpgradeDescriptionBuilder descriptionBuilder)
        {
            _currencyViewModel = currencyViewModel;
            _descriptionBuilder = descriptionBuilder;
            _currencyViewModel.CurrencyChanged += Render;
            Render(_currencyViewModel.GetCurrentAmount());
        }

        private void Render(int currentCurrency) =>
            _amount.text = _descriptionBuilder.GetCurrencyText(currentCurrency);
    }
}