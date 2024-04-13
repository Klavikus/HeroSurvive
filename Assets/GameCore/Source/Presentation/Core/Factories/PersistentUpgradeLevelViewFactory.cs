﻿using System;
using GameCore.Source.Domain.Services;
using GameCore.Source.Presentation.Api.Factories;
using GameCore.Source.Presentation.Api.GameLoop;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace GameCore.Source.Presentation.Core.Factories
{
    public class PersistentUpgradeLevelViewFactory : IPersistentUpgradeLevelViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        public PersistentUpgradeLevelViewFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public IUpgradeLevelView[] Create(int count)
        {
            IUpgradeLevelView[] result = new IUpgradeLevelView[count];

            for (int i = 0; i < count; i++)
                result[i] = Object
                    .Instantiate(_configurationProvider.UpgradeLevelView)
                    .GetComponent<IUpgradeLevelView>();

            return result;
        }
    }
}