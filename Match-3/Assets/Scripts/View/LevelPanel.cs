using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtilitiesAndHelpers;

namespace View
{
    public class LevelPanel : UIElementBase
    {
        [SerializeField] private Button _levelButton;
        [SerializeField] private Image _levelImage;
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inactiveColor;
        [SerializeField] private BasicResizableComponent _resizableComponent;
        
        public void Initialize(int levelNumber, bool isActive ,Action<int> onLevelSelected)
        {
            _levelName.text = SharedData.GetLevelName(levelNumber);
            _levelButton.onClick.AddListener(() => { onLevelSelected?.Invoke(levelNumber);});
            _levelImage.color = isActive ? _activeColor : _inactiveColor;
            _levelButton.interactable = isActive;
            _resizableComponent.Activate(isActive);
        }
    }
}
