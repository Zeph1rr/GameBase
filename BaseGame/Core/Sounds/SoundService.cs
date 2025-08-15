using BaseGame.Core.Interfaces;
using UnityEngine;

namespace BaseGame.Core.Sounds
{
    public class SoundService
    {
        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;
        private readonly IAudioClipProvider _clipProvider;

        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;
        
        public SoundService(IAudioClipProvider clipProvider, float musicVolume, float sfxVolume)
        {
            _clipProvider = clipProvider;

            var go = new GameObject("SoundService");
            Object.DontDestroyOnLoad(go);

            _musicSource = go.AddComponent<AudioSource>();
            _musicSource.loop = true;

            _sfxSource = go.AddComponent<AudioSource>();
            
            SetMusicVolume(musicVolume);
            SetSfxVolume(sfxVolume);
        }
        
        public void PlayMusic(string clipPath, bool loop = true)
        {
            var clip = _clipProvider.GetClip(clipPath);
            if (clip == null) return;

            _musicSource.loop = loop;
            _musicSource.clip = clip;
            _musicSource.volume = _musicVolume;
            _musicSource.Play();
        }

        public void PlaySfx(string clipPath)
        {
            var clip = _clipProvider.GetClip(clipPath);
            if (clip == null) return;

            _sfxSource.PlayOneShot(clip, _sfxVolume);
        }

        public void StopMusic() => _musicSource.Stop();

        public void SetMusicVolume(float volume)
        {
            if (!IsValidVolume(volume))
            {
                Debug.LogWarning($"[SoundService] Invalid music volume: {volume}. Must be between 0 and 1.");
                return;
            }

            _musicVolume = Mathf.Clamp01(volume);
            _musicSource.volume = _musicVolume;
        }

        public void SetSfxVolume(float volume)
        {
            if (!IsValidVolume(volume))
            {
                Debug.LogWarning($"[SoundService] Invalid SFX volume: {volume}. Must be between 0 and 1.");
                return;
            }

            _sfxVolume = Mathf.Clamp01(volume);
        }

        private bool IsValidVolume(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                return false;
            return value >= 0f && value <= 1f;
        }

    }
}