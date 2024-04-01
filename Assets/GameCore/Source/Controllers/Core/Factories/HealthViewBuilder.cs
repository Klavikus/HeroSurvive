using System;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Presentation.Api;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class HealthViewBuilder : IHealthViewBuilder
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public HealthViewBuilder(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
        }

        public void Build(GameObject objectWithHealthView)
        {
            IHealthView view = objectWithHealthView.GetComponent<IHealthView>();
            HealthPresenter presenter = new HealthPresenter(view, _coroutineRunner);
            view.Construct(presenter);
        }
    }
}