using System;
using System.Collections.Generic;
using BaseGame.Core.Interfaces;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace BaseGame.Core.LocalizationSystem
{
    public class ResourcesJsonLocalizationProvider: ILocalizationProvider
    {
        public event Action OnLanguageChanged;
        
        private readonly string _resourcesFolderPath;
        private Dictionary<string, string> _currentLanguageData = new Dictionary<string, string>();

        public ResourcesJsonLocalizationProvider(string resourcesFolderPath = "Localization")
        {
            _resourcesFolderPath = resourcesFolderPath;
        }




        public void LoadLanguage(string languageCode)
        {
            var jsonAsset = Resources.Load<TextAsset>($"{_resourcesFolderPath}/{languageCode}");
            if (jsonAsset == null)
            {
                Debug.LogWarning($"[Localization] File not found in Resources: {_resourcesFolderPath}/{languageCode}");
                _currentLanguageData.Clear();
                OnLanguageChanged?.Invoke();
                return;
            }
            _currentLanguageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonAsset.text);
            OnLanguageChanged?.Invoke();
        }

        public string Get(string key, string defaultValue = "")
        {
            return _currentLanguageData.GetValueOrDefault(key, defaultValue);
        }
    }
}