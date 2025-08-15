using System.Linq;
using System.Threading.Tasks;
using BaseGame.Core.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseGame.Core.Scenes
{
    public class SceneService
    {
        public async Task<ISceneEntryPoint> LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);

            while (!loadOperation.isDone)
            {
                await Task.Yield();
            }
            
            var allMonoBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            var sceneEntryPoint = allMonoBehaviours.OfType<ISceneEntryPoint>().FirstOrDefault();

            return sceneEntryPoint;
        }
        
        public async Task<ISceneEntryPoint> ReloadCurrentSceneAsync()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            return await LoadSceneAsync(currentScene);
        }
    }
}