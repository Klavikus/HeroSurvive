using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class SaveInfoService : ISaveInfoService
    {
        private const string FileName = "GameData.save";
        
        private readonly string _saveFileName;

        private Dictionary<string, int> _dataDictionary;

        public SaveInfoService()
        {
            _saveFileName = Application.persistentDataPath + FileName;
            _dataDictionary = new Dictionary<string, int>();
        }

        public void Clear()
        {
            _dataDictionary.Clear();
            SaveData();
        }

        public Dictionary<string, int> GetDataDict() =>
            _dataDictionary;

        public int TryGetData(string key, int defaultValue)
        {
            if (_dataDictionary.ContainsKey(key))
                return _dataDictionary[key];
            return defaultValue;
        }


        public void AddDataToDict(string key, int value)
        {
            if (_dataDictionary.ContainsKey(key))
                _dataDictionary[key] = value;
            else
                _dataDictionary.Add(key, value);
            SaveData();
        }

        public void LoadData() => 
            _dataDictionary = DeserializeData<Dictionary<string, int>>(_saveFileName) ?? new Dictionary<string, int>();

        public void SaveData() =>
            SerializeData(_dataDictionary, _saveFileName);

        private static void SerializeData<T>(T data, string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            
            try
            {
                formatter.Serialize(fileStream, data);
            }
            catch (SerializationException e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }

        private static T DeserializeData<T>(string path)
        {
            T data = default(T);

            if (!File.Exists(path)) 
                return data;
            
            FileStream fileStream = new FileStream(path, FileMode.Open);
            
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                data = (T) formatter.Deserialize(fileStream);
            }
            catch (SerializationException e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                fileStream.Close();
            }

            return data;
        }
    }
}