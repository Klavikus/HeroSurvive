using System;
using CodeBase.Domain;
using CodeBase.Domain.Data;
using CodeBase.MVVM.Views;
using CodeBase.MVVM.Views.HeroSelector;

namespace CodeBase.Infrastructure.Services.PropertiesProviders
{
    public interface IPropertyProvider : IService
    {
        event Action PropertiesUpdated;
        void Initialize();
        MainProperties GetResultProperties();
        MainPropertyViewData[] GetResultPropertiesViewData();
        PropertyView GetBasePropertyView();
    }
}