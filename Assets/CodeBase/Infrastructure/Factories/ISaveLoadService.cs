using System;

namespace CodeBase.Infrastructure
{
    public interface ISaveLoadService
    {
        event Action AllLoaded;
        bool ContainData(string dataKey);
        string GetData(string dattaKey);
        void SaveToData(string dattaKey, string json);
        void SaveDataToPrefs();
        void LoadAllDataFromYandex();
        void LoadPrefsToData();
    }
}