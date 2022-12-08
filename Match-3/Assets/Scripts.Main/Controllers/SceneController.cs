using Scripts.CommonExtensions.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scripts.Main.Controllers
{
    public interface ILoadScene
    {
        void LoadScene(int sceneId);
    }

    public class SceneController : ILoadScene
    {
        private GameObject _loadCanvas;
        
        public SceneController(GameObject loadCanvas)
        {
            _loadCanvas = loadCanvas;
            Object.DontDestroyOnLoad(loadCanvas);
        }
        
        public void LoadScene(int targetIndex)
        {
            _loadCanvas.SetActiveOptimize(true);
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var loadSceneAsync = SceneManager.LoadSceneAsync(targetIndex, LoadSceneMode.Additive);

            loadSceneAsync.completed += loadOperation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(targetIndex));
                var unloadOperation = SceneManager.UnloadSceneAsync(currentScene);
                unloadOperation.completed += unloadOperation =>
                {
                    _loadCanvas.SetActiveOptimize(false);
                };
            };
        }
    }
}