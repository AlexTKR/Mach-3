using System;
using Composition;
using Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using View;
using Zenject;
using IProcessPanel = Controllers.IProcessPanel;

namespace Root
{
    public abstract class RootInitiator : MonoBehaviour
    {
        protected IInitBehaviour _initer;
        protected ISetPanelProcessor _setPanelProcessor;
        protected IPanelProcessor _panelProcessor;

        [Inject]
        private void Construct(IInitBehaviour initer, ISetPanelProcessor setPanelProcessor,
            IPanelProcessor panelProcessor)
        {
            _initer = initer;
            _setPanelProcessor = setPanelProcessor;
            _panelProcessor = panelProcessor;
        }

        protected async void Awake()
        {
            if (_initer is null)
                return;

            await _initer.InitViews();
        }

        protected virtual async void Start()
        {
            _setPanelProcessor.SetProcessor(_panelProcessor);

            if (_initer is null)
                return;

            await _initer.InitControllers();
        }
    }

    public class MainSceneRoot : RootInitiator
    {
        private ILoadGameSettings _loadGameSettings;
        private ILoadLevel<LevelData> _loadLevel;
        private IGetLevelData _getLevelData;
        private IGetEventSystem _getEventSystem;
        private MainCanvas _mainCanvas;

        [Inject]
        private void Construct(ILoadGameSettings loadGameSettings,
            ILoadLevel<LevelData> loadLevel, IGetLevelData getLevelData,
            IGetEventSystem getEventSystem)
        {
            _loadGameSettings = loadGameSettings;
            _loadLevel = loadLevel;
            _getLevelData = getLevelData;
            _getEventSystem = getEventSystem;
        }

        protected override void Start()
        {
#if UNITY_EDITOR

            if (FindObjectOfType<EventSystem>() is null)
                DontDestroyOnLoad(MonoBehaviour.Instantiate(_getEventSystem.GetEventSystem()));
#endif
            _loadGameSettings.LoadGameSettings();

            base.Start();
            _loadLevel.LoadLevel(_getLevelData.GetCurrentLevelData());
        }
    }
}