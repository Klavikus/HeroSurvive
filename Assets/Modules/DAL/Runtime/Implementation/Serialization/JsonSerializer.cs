using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Modules.DAL.Abstract.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Modules.DAL.Implementation.Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly IList<Type> _knownTypes;

        public JsonSerializer(IEnumerable<Type> knownTypes)
        {
            _knownTypes = knownTypes.ToList();
        }

        public string Serialize(object @object)
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            if (_knownTypes.Contains(@object.GetType()) == false)
                throw new ArgumentException(nameof(@object));

            string json = JsonConvert.SerializeObject(@object, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new DatasetEntityTypesBinder()
                {
                    KnownTypes = _knownTypes
                },
                Formatting = Formatting.None,
            });

            byte[] compressedData = CompressString(json);

            ToJsonDecor decor = new ToJsonDecor(compressedData);
            json = JsonConvert.SerializeObject(decor);

            return json;
        }

        public T Deserialize<T>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return default;

            if (TryParseJson(jsonString, out ToJsonDecor jsonDecor) == false)
            {
                Debug.LogWarning("Json string is invalid - return default object!");

                return default;
            }

            string decompressedString = DecompressString(jsonDecor.Data);

            T fromJson = default;

            try
            {
                if (!string.IsNullOrWhiteSpace(decompressedString))
                {
                    fromJson = JsonConvert.DeserializeObject<T>(decompressedString, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        SerializationBinder = new DatasetEntityTypesBinder()
                        {
                            KnownTypes = _knownTypes
                        },
                        Formatting = Formatting.None,
                    });
                }
            }
            catch (JsonReaderException)
            {
                Debug.Log(nameof(JsonReaderException));
            }

            return fromJson;
        }

        private bool TryParseJson<T>(string json, out T result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(json);

                return true;
            }
            catch
            {
                result = default;

                return false;
            }
        }

        private static byte[] CompressString(string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);

            using (var outputMemoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputMemoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(byteArray, 0, byteArray.Length);
                }

                return outputMemoryStream.ToArray();
            }
        }

        private static string DecompressString(byte[] compressedData)
        {
            if (compressedData == null)
                return string.Empty;

            using (var inputMemoryStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(inputMemoryStream, CompressionMode.Decompress))
            using (var outputMemoryStream = new MemoryStream())
            {
                gzipStream.CopyTo(outputMemoryStream);
                byte[] decompressedByteArray = outputMemoryStream.ToArray();

                return Encoding.UTF8.GetString(decompressedByteArray);
            }
        }

        private struct ToJsonDecor
        {
            public readonly byte[] Data;

            public ToJsonDecor(byte[] data) =>
                Data = data;
        }
    }
}