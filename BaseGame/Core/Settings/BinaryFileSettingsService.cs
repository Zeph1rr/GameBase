using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BaseGame.Core.Settings
{
    public class BinaryFileSettingsService: Settings.SettingsService
    {
        private readonly string _filePath;

        public BinaryFileSettingsService(string fileName = "settings.dat")
        {
            _filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        protected override void SaveToStorage(Dictionary<string, object> data)
        {
            try
            {
                using var fs = new FileStream(_filePath, FileMode.Create);
                var bf = new BinaryFormatter();
                bf.Serialize(fs, data);
            }
            catch (Exception e)
            {
                Debug.LogError($"[BinaryFileSettingsService] Ошибка при сохранении: {e}");
            }
        }

        protected override Dictionary<string, object> LoadFromStorage()
        {
            if (!File.Exists(_filePath))
                return new Dictionary<string, object>();

            try
            {
                using var fs = new FileStream(_filePath, FileMode.Open);
                var bf = new BinaryFormatter();
                return bf.Deserialize(fs) as Dictionary<string, object> ?? new Dictionary<string, object>();
            }
            catch (Exception e)
            {
                Debug.LogError($"[BinaryFileSettingsService] Ошибка при загрузке: {e}");
                return new Dictionary<string, object>();
            }
        }

        protected override T DeserializeValue<T>(object rawValue)
        {
            return rawValue is T castValue ? castValue : default;
        }
    }
}