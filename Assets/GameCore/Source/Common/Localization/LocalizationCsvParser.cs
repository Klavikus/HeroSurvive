using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Source.Common.Localization
{
    public static class LocalizationCsvParser
    {
        public static Dictionary<string, Dictionary<string, string>> Parse(string filePath, char separator = ',')
        {
            Dictionary<string, Dictionary<string, string>> localizationData =
                new Dictionary<string, Dictionary<string, string>>();

            TextAsset file = Resources.Load<TextAsset>(filePath);
            string content = file.text.Replace("\r", "");

            string[] strings = content.Split("\n");
            string[] languages = strings[0].Split(separator);

            for (int i = 1; i < strings.Length; i++)
            {
                string[] data = strings[i].Split(separator);

                string key = data[0];
                Dictionary<string, string> translations = new Dictionary<string, string>();

                for (int j = 1; j < data.Length; j++)
                {
                    if (!string.IsNullOrEmpty(data[j]))
                    {
                        translations[languages[j]] = data[j];
                    }
                }

                localizationData[key] = translations;
            }

            return localizationData;
        }
    }
}