using Scripts.Main.Controllers;
using Scripts.Main.Loadable;
using Scripts.Main.UtilitiesAndHelpers;
using UnityEngine;
using Zenject;

namespace Scripts.Composition.Root
{
    public class LoadSceneRoot : MonoBehaviour
    {
        private ILoadScene _loadScene;
        private IGetEventSystem _getEventSystem;

        [Inject]
        private void Construct(ILoadScene loadScene, IGetEventSystem getEventSystem)
        {
            _loadScene = loadScene;
            _getEventSystem = getEventSystem;
        }

        private async void Start()
        {
            var eventSystem = await _getEventSystem.GetEventSystem().Load();
            DontDestroyOnLoad(MonoBehaviour.Instantiate(eventSystem));
            _loadScene.LoadScene(SharedData.MapSceneIndex);
        }
    }
}