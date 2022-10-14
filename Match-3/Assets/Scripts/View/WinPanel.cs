using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public abstract class QuitPanel : PanelBase
    {
        [SerializeField] private Button _quitButton;
        
        private Action _onContinue;
        
        public override Task Initialize()
        {
            _quitButton.onClick.AddListener(() =>
            {
                _onContinue?.Invoke();
            });
            return base.Initialize();
        }
        
        public void SetQuitCallback(Action callback)
        {
            _onContinue += callback;
        }
    }

    public class WinPanel : QuitPanel
    {
        
    }
}
