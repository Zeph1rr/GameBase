using System.Collections.Generic;

namespace BaseGame.Core.Interfaces
{
    public interface ISaveLoadSystem
    {
        public void Save<T>(T saveData, string key);
        public T Load<T>(string key, T defaultData);
        
        public bool HasSave(string key);
        public void DeleteSave(string key);
        public void DeleteAllSaves();
        public IEnumerable<string> GetAllSaveKeys();
    }
}