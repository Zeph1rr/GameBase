namespace BaseGame.Core.Interfaces
{
    public interface ISaveLoadSystem
    {
        public void Save<T>(T saveData, string key);
        public T Load<T>(string key, T defaultData);
    }
}