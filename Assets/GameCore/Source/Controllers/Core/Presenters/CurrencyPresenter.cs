using System;
using GameCore.Source.Domain.Models;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class CurrencyPresenter : IPresenter
    {
        private readonly ICurrencyView _view;
        private readonly CurrencyModel _currencyModel;

        public CurrencyPresenter(ICurrencyView view, CurrencyModel currencyModel)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _currencyModel = currencyModel ?? throw new ArgumentNullException(nameof(currencyModel));
        }

        public void Enable()
        {
            _view.Counter.Initialize(_currencyModel.CurrentAmount);
            
            _currencyModel.CurrencyChanged += OnCurrencyChanged;
        }

        public void Disable()
        {
            _currencyModel.CurrencyChanged -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged(int currency) =>
            _view.Counter.Count(currency);
    }
}