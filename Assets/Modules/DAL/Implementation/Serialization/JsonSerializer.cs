using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Modules.DAL.Abstract.Serialization;
using Newtonsoft.Json;

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
            string decompressedString = DecompressString(JsonConvert.DeserializeObject<ToJsonDecor>(jsonString).Data);

            T fromJson = JsonConvert.DeserializeObject<T>(decompressedString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new DatasetEntityTypesBinder()
                {
                    KnownTypes = _knownTypes
                },
                Formatting = Formatting.None,
            });

            return fromJson;
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