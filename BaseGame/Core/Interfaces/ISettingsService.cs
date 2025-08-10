using System;

namespace BaseGame.Core.Interfaces
{
    public interface ISettingsService
    {
        public void Subscribe<T>(string key, Action<T> callback);
        public void Unsubscribe<T>(string key, Action<T> callback);

        public T Get<T>(string key, T defaultValue = default);
        public void Set<T>(string key, T value);
        
        public void Save();
        public void Load();
    }
}