using System.IO;
using Unity.Plastic.Newtonsoft.Json;

namespace BaseGame.Core.SaveLoad
{
    public sealed class JsonFileSaveLoadSystem : FileSaveLoadSystem
    {
        public JsonFileSaveLoadSystem(string folderName = "JsonSaves") : base(folderName) { }

        public override void Save<T>(T saveData, string key)
        {
            var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(GetFilePath(key), json);
        }

        public override T Load<T>(string key, T defaultData)
        {
            var path = GetFilePath(key);
            if (!File.Exists(path))
            {
                return defaultData;
            }

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected override string GetFileExtension()
        {
            return ".json";
        }
    }
}