using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame.Core.Settings
{
    public class PlayerPrefsSettingsService: Settings.SettingsService
    {
        protected override void SaveToStorage(Dictionary<string, object> data)
        {
            foreach (var kvp in data)
                PlayerPrefs.SetString(kvp.Key, JsonUtility.ToJson(new Wrapper(kvp.Value)));

            PlayerPrefs.Save();
        }

        protected override Dictionary<string, object> LoadFromStorage()
        {
            var dict = new Dictionary<string, object>();

            // PlayerPrefs не даёт получить список ключей, так что их надо хранить отдельно
            var keys = PlayerPrefs.GetString("__SettingsKeys", "");
            if (string.IsNullOrEmpty(keys))
                return dict;

            foreach (var key in keys.Split('|'))
            {
                if (!string.IsNullOrEmpty(key) && PlayerPrefs.HasKey(key))
                {
                    var json = PlayerPrefs.GetString(key);
                    var wrapper = JsonUtility.FromJson<Wrapper>(json);
                    dict[key] = wrapper?.Value;
                }
            }
            return dict;
        }

        protected override T DeserializeValue<T>(object rawValue)
        {
            return rawValue is T castValue ? castValue : default;
        }

        [Serializable]
        private class Wrapper
        {
            public object Value;
            public Wrapper(object value) { Value = value; }
        }
    }
}