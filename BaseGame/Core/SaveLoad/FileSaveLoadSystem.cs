using System.Collections.Generic;
using System.IO;
using BaseGame.Core.Interfaces;
using UnityEngine;

namespace BaseGame.Core.SaveLoad
{
    public abstract class FileSaveLoadSystem : ISaveLoadSystem
    {
        private readonly string _saveFolderPath;

        protected FileSaveLoadSystem(string folderName)
        {
            _saveFolderPath = Path.Combine(Application.persistentDataPath, folderName);
            if (!Directory.Exists(_saveFolderPath))
            {
                Directory.CreateDirectory(_saveFolderPath);
            }
        }

        public abstract void Save<T>(T saveData, string key);
        public abstract T Load<T>(string key, T defaultData);

        public bool HasSave(string key)
        {
            var path = GetFilePath(key);
            return File.Exists(path);
        }

        public void DeleteSave(string key)
        {
            var path = GetFilePath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void DeleteAllSaves()
        {
            if (!Directory.Exists(_saveFolderPath)) return;
            Directory.Delete(_saveFolderPath, true);
            Directory.CreateDirectory(_saveFolderPath);
        }

        public IEnumerable<string> GetAllSaveKeys()
        {
            if (!Directory.Exists(_saveFolderPath))
            {
                yield break;
            }

            foreach (var file in Directory.GetFiles(_saveFolderPath))
            {
                yield return Path.GetFileNameWithoutExtension(file);
            }
        }

        protected string GetFilePath(string key)
        {
            return Path.Combine(_saveFolderPath, key + GetFileExtension());
        }

        protected abstract string GetFileExtension();
    }
}