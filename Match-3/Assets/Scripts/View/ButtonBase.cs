using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public abstract class ButtonBase : MonoBehaviour
    {
        [SerializeField] protected Button _button;

        protected Action _onClick;

        protected virtual void Awake()
        {
            _button.onClick.AddListener(() =>
            {
                _onClick?.Invoke();
            });
        }

        public void AddOnClickEvent(Action action)
        {
            _onClick += action;
        }
        
        public void RemoveOnClickEvent(Action action)
        {
            _onClick -= action;
        }
    }
}
