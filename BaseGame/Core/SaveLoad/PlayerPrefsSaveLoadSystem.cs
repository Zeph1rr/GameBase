using System;
using System.Collections.Generic;
using BaseGame.Core.Interfaces;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace BaseGame.Core.SaveLoad
{
    public sealed class PlayerPrefsSaveLoadSystem : ISaveLoadSystem
    {
        public void Save<T>(T saveData, string key)
        {
            string json = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key, T defaultData)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultData;
            }

            var json = PlayerPrefs.GetString(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public bool HasSave(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void DeleteSave(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void DeleteAllSaves()
        {
            PlayerPrefs.DeleteAll();
        }

        public IEnumerable<string> GetAllSaveKeys()
        {
            Debug.LogWarning("PlayerPrefsSaveLoadSystem does not support listing all keys directly.");
            return Array.Empty<string>();
        }
    }
}