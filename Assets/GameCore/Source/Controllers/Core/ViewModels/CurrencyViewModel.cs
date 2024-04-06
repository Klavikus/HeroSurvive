using System;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Core.ViewModels
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

        ~CurrencyViewModel()
        {
            _currencyModel.CurrencyChanged -= OnCurrencyModelChanged;
        }

        private void OnCurrencyModelChanged(int currentCurrency) =>
            CurrencyChanged?.Invoke(currentCurrency);

        public int GetCurrentAmount() =>
            _currencyModel.CurrentAmount;

        public void ApplyAdditionalReward(int gainedCurrency) =>
            _currencyModel.Add(gainedCurrency);
    }
}