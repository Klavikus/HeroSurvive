using System;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using UnityEngine;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IPropertyProvider
    {
        event Action PropertiesUpdated;
        void Initialize();
        MainProperties GetResultProperties();
        MainPropertyViewData[] GetResultPropertiesViewData();
        GameObject GetBasePropertyView();
    }
}