using System;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.Factories
{
    public interface ISaveLoadService : IService
    {
        event Action AllLoaded;
        bool ContainData(string dataKey);
        string GetData(string dattaKey, string defaultValue = "");
        void SaveToData(string dattaKey, string json);
        void SaveDataToPrefs();
        void LoadAllDataFromYandex();
        void LoadPrefsToData();
    }
}