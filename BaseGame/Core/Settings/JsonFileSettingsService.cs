using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace BaseGame.Core.Settings
{
    public class JsonFileSettingsService: Settings.SettingsService
    {
        private readonly string _filePath;

        public JsonFileSettingsService(string fileName = "settings.json")
        {
            _filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        protected override void SaveToStorage(Dictionary<string, object> data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(_filePath, json, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonFileSettingsService] Ошибка при сохранении: {e}");
            }
        }

        protected override Dictionary<string, object> LoadFromStorage()
        {
            if (!File.Exists(_filePath))
                return new Dictionary<string, object>();

            try
            {
                var json = File.ReadAllText(_filePath, Encoding.UTF8);
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(json)
                       ?? new Dictionary<string, object>();
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonFileSettingsService] Ошибка при загрузке: {e}");
                return new Dictionary<string, object>();
            }
        }

        protected override T DeserializeValue<T>(object rawValue)
        {
            return rawValue is T castValue
                ? castValue
                : JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(rawValue));
        }
    }
}