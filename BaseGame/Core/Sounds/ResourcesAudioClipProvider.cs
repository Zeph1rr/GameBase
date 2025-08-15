using System.Collections.Generic;
using BaseGame.Core.Interfaces;
using UnityEngine;

namespace BaseGame.Core.Sounds
{
    public class ResourcesAudioClipProvider: IAudioClipProvider
    {
        private readonly Dictionary<string, AudioClip> _cache = new();
        private readonly string _basePath;

        public ResourcesAudioClipProvider(string basePath = "")
        {
            _basePath = basePath;
        }

        public AudioClip GetClip(string clipPath)
        {
            var fullPath = string.IsNullOrEmpty(_basePath) ? clipPath : $"{_basePath}/{clipPath}";

            if (_cache.TryGetValue(fullPath, out var cached))
                return cached;

            var clip = Resources.Load<AudioClip>(fullPath);
            if (clip != null)
            {
                _cache[fullPath] = clip;
            }
            else
            {
                Debug.LogWarning($"[ResourcesAudioClipProvider] Audio clip '{fullPath}' not found in Resources.");
            }

            return clip;
        }
    }
}