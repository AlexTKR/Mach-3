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
        Task LoadScene(int sceneId);
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

        public async Task LoadScene(int targetIndex)
        {
            _loadCanvas.Enable();
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var loadSceneAsync = SceneManager.LoadSceneAsync(targetIndex, LoadSceneMode.Additive);

            loadSceneAsync.completed += operation =>
            {
                _processPanel.ProcessSceneChanged();
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(targetIndex));
                SceneManager.UnloadSceneAsync(currentScene);
                _loadCanvas.Disable();
            };
        }
    }
}