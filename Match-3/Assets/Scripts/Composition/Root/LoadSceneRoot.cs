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

        protected void Start()
        {
             DontDestroyOnLoad(MonoBehaviour.Instantiate(_getEventSystem.GetEventSystem()));
            _loadScene.LoadScene(SharedData.MapSceneIndex);
        }
    }
}