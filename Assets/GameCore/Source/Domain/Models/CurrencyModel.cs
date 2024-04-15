using System;

namespace GameCore.Source.Domain.Models
{
    public class CurrencyModel
    {
        private int _currentCurrency;
        
        public event Action<int> CurrencyChanged;
        
        public int CurrentAmount => _currentCurrency;
        
        public bool CheckPayAvailability(int price) =>
            _currentCurrency >= price;

        public void Pay(int price)
        {
            if (CheckPayAvailability(price) == false)
                throw new ArgumentException($"{_currentCurrency} should be greater then pay price");

            UpdateCurrency(-price);
        }

        public void Add(int price)
        {
            if (price <= 0)
                throw new ArgumentException($"{nameof(price)} should be greater than 0");

            UpdateCurrency(price);
        }

        private void UpdateCurrency(int currency)
        {
            _currentCurrency += currency;
            CurrencyChanged?.Invoke(_currentCurrency);
        }

        public void SetAmount(int newValue)
        {
            _currentCurrency = newValue;
            CurrencyChanged?.Invoke(_currentCurrency);
        }
    }
}