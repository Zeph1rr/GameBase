using System;
using System.Collections.Generic;
using BaseGame.Core.Interfaces;

namespace BaseGame.Core.LocalizationSystem
{
    public static class Localization
    {
        private static ILocalizationProvider _provider;

        public static event Action OnLanguageChanged;

        public static void Init(ILocalizationProvider provider)
        {
            if (_provider != null)
            {
                _provider.OnLanguageChanged -= RaiseLanguageChanged;
            }

            _provider = provider;
            _provider.OnLanguageChanged += RaiseLanguageChanged;
        }

        private static void RaiseLanguageChanged()
        {
            OnLanguageChanged?.Invoke();
        }

        public static string Get(string key, string defaultValue = "")
        {
            return _provider == null ? defaultValue : _provider.Get(key, defaultValue);
        }

        public static void LoadLanguage(string languageCode)
        {
            if (_provider == null)
            {
                throw new InvalidOperationException("Localization provider not initialized");
            }

            _provider.LoadLanguage(languageCode);
        }
    }
}