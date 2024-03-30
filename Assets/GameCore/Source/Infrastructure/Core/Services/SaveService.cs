using System;
using GameCore.Source.Infrastructure.Api.Services;
using UnityEngine;

namespace GameCore.Source.Infrastructure.Core.Services
{
    public class SaveService : ISaveService
    {
        public void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public string Get(string key) => 
            PlayerPrefs.GetString(key, String.Empty);

        public void Clear() => 
            PlayerPrefs.DeleteAll();
    }
}