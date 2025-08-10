using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BaseGame.Core.SaveLoad
{
    public sealed class BinaryFileSaveLoadSystem : FileSaveLoadSystem
    {
        public BinaryFileSaveLoadSystem(string folderName = "BinarySaves") : base(folderName) { }

        public override void Save<T>(T saveData, string key)
        {
            using var fs = new FileStream(GetFilePath(key), FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(fs, saveData);
        }

        public override T Load<T>(string key, T defaultData)
        {
            var path = GetFilePath(key);
            if (!File.Exists(path))
            {
                return defaultData;
            }

            using var fs = new FileStream(path, FileMode.Open);
            var bf = new BinaryFormatter();
            var obj = bf.Deserialize(fs);
            return obj is T value ? value : defaultData;
        }

        protected override string GetFileExtension()
        {
            return ".bin";
        }
    }
}