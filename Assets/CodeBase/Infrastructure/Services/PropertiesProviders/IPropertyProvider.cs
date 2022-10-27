using System;
using CodeBase.HeroSelection;
using CodeBase.MVVM.Views;

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