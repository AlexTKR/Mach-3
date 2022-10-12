using System;
using Composition;
using Controllers;
using UnityEngine;
using Zenject;

namespace Root
{
    public class MainSceneRoot : MonoBehaviour
    {
        private IInitBehaviour _initer;
        private ILoadGameSettings _loadGameSettings;
        private ILoadLevel _loadLevel;
        private ILevelData _levelData;

        [Inject]
        private void Construct(IInitBehaviour initer, ILoadGameSettings loadGameSettings,
            ILoadLevel loadLevel, ILevelData levelData)
        {
            _initer = initer;
            _loadGameSettings = loadGameSettings;
            _loadLevel = loadLevel;
            _levelData = levelData;
        }

        private async void Awake()
        {
            await _initer.InitViews();
        }

        private async void Start()
        {
            _loadGameSettings.LoadGameSettings();
            await _initer.InitControllers();
            _loadLevel.LoadLevel(_levelData.GetCurrentLevelData());
        }
    }
}