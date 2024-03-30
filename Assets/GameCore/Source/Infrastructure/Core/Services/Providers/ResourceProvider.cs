using System;
using System.Collections.Generic;
using Source.Infrastructure.Api;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Infrastructure.Core.Services.Providers
{
    public class ResourceProvider : IResourceProvider
    {
        private static readonly Dictionary<Type, string> ResourcePathByType = new()
        {
            [typeof(ConfigurationContainer)] = "ConfigurationContainer",
        };

        public T Load<T>() where T : Object
        {
            Debug.Log($"Load asset from {ResourcePathByType[typeof(T)]}");

            return Resources.Load<T>(ResourcePathByType[typeof(T)]);
        }
    }
}