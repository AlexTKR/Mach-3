using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UtilitiesAndHelpers;
using View;
using Zenject;

namespace Controllers
{
    public interface ILoadScene
    {
        void LoadScene(int sceneId);
    }

    public class SceneController : ILoadScene
    {
        private LoadCanvas _loadCanvas;
        private IProcessPanel _processPanel;

        [Inject]
        private void Construct(LoadCanvas loadCanvas, IProcessPanel processPanel)
        {
            _loadCanvas = loadCanvas;
            _processPanel = processPanel;
        }

        public void LoadScene(int targetIndex)
        {
            _loadCanvas.Enable();
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var loadSceneAsync = SceneManager.LoadSceneAsync(targetIndex, LoadSceneMode.Additive);

            loadSceneAsync.completed += loadOperation =>
            {
                _processPanel.ProcessSceneChanged();
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(targetIndex));
                var unloadOperation = SceneManager.UnloadSceneAsync(currentScene);
                unloadOperation.completed += unloadOperation =>
                {
                    _loadCanvas.Disable();
                };
            };
        }
    }
}