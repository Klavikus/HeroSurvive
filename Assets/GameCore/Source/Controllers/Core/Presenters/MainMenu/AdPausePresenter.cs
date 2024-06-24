using System;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters.MainMenu
{
    public class AdPausePresenter : IPresenter
    {
        private readonly IAdPauseView _view;
        private readonly IAdsProvider _adsProvider;

        public AdPausePresenter(IAdPauseView view, IAdsProvider adsProvider)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _adsProvider = adsProvider ?? throw new ArgumentNullException(nameof(adsProvider));
        }

        public void Enable()
        {
            _adsProvider.AdStarted += _view.Show;
            _adsProvider.AdClosed += _view.Hide;

            if (_adsProvider.IsAdInProgress)
                _view.Show();
            else
                _view.Hide();
        }

        public void Disable()
        {
            _adsProvider.AdStarted -= _view.Show;
            _adsProvider.AdClosed -= _view.Hide;
        }
    }
}