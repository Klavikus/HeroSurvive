using Modules.MVPPassiveView._Samples.Domain;
using Modules.MVPPassiveView._Samples.Views;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace Modules.MVPPassiveView._Samples.Presenters
{
    public class SamplePresenter : IPresenter
    {
        private readonly ISampleView _view;
        private readonly SampleModel _sampleModel;

        public SamplePresenter(ISampleView view, SampleModel sampleModel)
        {
            _view = view;
            _sampleModel = sampleModel;
        }

        public void Enable()
        {
            UpdateViewClicksCount(_sampleModel.ClicksCount);
            _sampleModel.ClicksCountChanged += UpdateViewClicksCount;
            _view.SampleButton.onClick.AddListener(OnSampleButtonClicked);
        }

        public void Disable()
        {
            _sampleModel.ClicksCountChanged -= UpdateViewClicksCount;
            _view.SampleButton.onClick.RemoveListener(OnSampleButtonClicked);
        }

        private void OnSampleButtonClicked()
        {
            Debug.Log("Sample Button Clicked!");
            _sampleModel.HandleClick();
        }

        private void UpdateViewClicksCount(int clicksCount) =>
            _view.SetClicksCountText(clicksCount.ToString());
    }
}