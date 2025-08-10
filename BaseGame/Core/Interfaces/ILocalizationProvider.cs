using System;

namespace BaseGame.Core.Interfaces
{
    public interface ILocalizationProvider
    {
        public event Action OnLanguageChanged;
        
        public void LoadLanguage(string languageCode);
        public string Get(string key, string defaultValue = "");
    }
}