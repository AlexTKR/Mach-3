using Controllers;
using UnityEngine;
using UtilitiesAndHelpers;
using Zenject;

namespace Composition.Root
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
             _getEventSystem.GetEventSystem().Release();
            _loadScene.LoadScene(SharedData.MapSceneIndex);
        }
    }
}