using System;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Presentation.Views.HeroSelector;

namespace CodeBase.GameCore.Infrastructure.Services.PropertiesProviders
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