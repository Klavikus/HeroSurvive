using System.Collections.Generic;

namespace CodeBase.Infrastructure
{
    public interface ISaveInfoService : IService
    {
        public Dictionary<string, int> GetDataDict();
        public void AddDataToDict(string key, int value);
        public void LoadData();
        public void SaveData();
        public void Clear();
        public int TryGetData(string key, int defaultValue);
    }
}