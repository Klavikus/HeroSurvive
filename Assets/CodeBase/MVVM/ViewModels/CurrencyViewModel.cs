using System;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class CurrencyViewModel
    {
        private readonly CurrencyModel _currencyModel;

        public event Action<int> CurrencyChanged;

        public CurrencyViewModel(CurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;

            _currencyModel.CurrencyChanged += OnCurrencyModelChanged;
        }

        private void OnCurrencyModelChanged(int currentCurrency) =>
            CurrencyChanged?.Invoke(currentCurrency);

        public int GetCurrentAmount() => _currencyModel.CurrentAmount;
    }
}