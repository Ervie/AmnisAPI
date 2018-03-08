using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace API.Utilities
{
    public class JSONSerializer
    {
        private readonly string resourcesPath;

        public JSONSerializer()
        {
            resourcesPath = AppContext.BaseDirectory + "/Resources/";
        }

        public List<T> LoadFromFile<T>(string fileName)
        {
            var text = File.ReadAllText(resourcesPath + fileName, Encoding.UTF8);

            var deserializedList = JsonConvert.DeserializeObject<List<T>>(text);

            return deserializedList ?? new List<T>();
        }

        public void SaveToFile<T>(string fileName, List<T> dataToSave)
        {
            var json = JsonConvert.SerializeObject(dataToSave);
            File.WriteAllText(resourcesPath + fileName, json, Encoding.UTF8);
        }
    }
}
