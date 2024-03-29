using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Infrastructure.Services
{
    public class ViewModelProvider : IViewModelProvider
    {
        private readonly Dictionary<Type, object> _viewModelByType = new Dictionary<Type, object>();

        public void Bind<TModel>(TModel model) where TModel : class
        {
            _viewModelByType[typeof(TModel)] = model;
        }

        public TModel Get<TModel>() where TModel : class =>
            _viewModelByType[typeof(TModel)] as TModel;
    }
}