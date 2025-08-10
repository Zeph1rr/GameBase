using System;
using System.Collections.Generic;

namespace BaseGame.Core.Settings
{
    public abstract class SettingsService
    {
        private readonly Dictionary<string, List<Action<object>>> _keySubscribers = new();
    private readonly Dictionary<(string key, Delegate original), Action<object>> _typedWrappers = new();

    protected Dictionary<string, object> _settings = new();

    public void Subscribe<T>(string key, Action<T> callback)
    {
        Action<object> wrapper = obj =>
        {
            if (obj is T value)
                callback(value);
            else
                callback(Get(key, default(T)));
        };

        _typedWrappers[(key, callback)] = wrapper;

        if (!_keySubscribers.ContainsKey(key))
            _keySubscribers[key] = new List<Action<object>>();

        _keySubscribers[key].Add(wrapper);
    }

    public void Unsubscribe<T>(string key, Action<T> callback)
    {
        if (_typedWrappers.TryGetValue((key, callback), out var wrapper))
        {
            if (_keySubscribers.TryGetValue(key, out var list))
            {
                list.Remove(wrapper);
                if (list.Count == 0)
                    _keySubscribers.Remove(key);
            }
            _typedWrappers.Remove((key, callback));
        }
    }

    public T Get<T>(string key, T defaultValue = default)
    {
        if (_settings.TryGetValue(key, out var value))
        {
            try
            {
                return value is T castValue
                    ? castValue
                    : DeserializeValue<T>(value);
            }
            catch
            {
                return defaultValue;
            }
        }
        return defaultValue;
    }

    public void Set<T>(string key, T value)
    {
        _settings[key] = value;

        if (_keySubscribers.TryGetValue(key, out var list))
        {
            foreach (var callback in list)
                callback(value);
        }
    }

    public void Save()
    {
        SaveToStorage(_settings);
    }

    public void Load()
    {
        _settings = LoadFromStorage() ?? new Dictionary<string, object>();

        foreach (var kvp in _settings)
        {
            if (_keySubscribers.TryGetValue(kvp.Key, out var list))
            {
                foreach (var callback in list)
                    callback(kvp.Value);
            }
        }
    }

    protected abstract void SaveToStorage(Dictionary<string, object> data);
    protected abstract Dictionary<string, object> LoadFromStorage();
    protected abstract T DeserializeValue<T>(object rawValue);
    }
}