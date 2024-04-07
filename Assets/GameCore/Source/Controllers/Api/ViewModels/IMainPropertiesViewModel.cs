using System;
using System.Collections.Generic;
using GameCore.Source.Domain.Enums;

namespace GameCore.Source.Controllers.Api.ViewModels
{
    public interface IMainPropertiesViewModel
    {
        event Action<IReadOnlyDictionary<BaseProperty, float>> PropertiesChanged;
        IReadOnlyDictionary<BaseProperty, float> BaseProperties { get; }
    }
}