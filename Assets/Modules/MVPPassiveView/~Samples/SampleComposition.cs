using Modules.MVPPassiveView._Samples.Domain;
using Modules.MVPPassiveView._Samples.Presenters;
using Modules.MVPPassiveView._Samples.Views;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace Modules.MVPPassiveView._Samples
{
    public class SampleComposition : MonoBehaviour
    {
        [SerializeField] private SampleView _sampleView;

        public void Start()
        {
            SampleModel sampleModel = new SampleModel();
            IPresenter samplePresenter = new SamplePresenter(_sampleView, sampleModel);
            _sampleView.Construct(samplePresenter);
        }
    }
}