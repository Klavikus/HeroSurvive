using System;
using CodeBase.Domain;
using CodeBase.Presentation;

namespace CodeBase.Infrastructure
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