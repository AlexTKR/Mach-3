using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PausePanel : QuitPanel
    {
        [SerializeField] private Button _playOnButton;
        
        public override Task Initialize()
        {
            _playOnButton.onClick.AddListener((Disable));
            return base.Initialize();
        }
        
    }
}
