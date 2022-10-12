using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UtilitiesAndHelpers;

namespace View
{
    public class ResizableViews
    {
        public interface IPointerProcessor
        {
            void ProcessUp(Transform target, Vector3 scale, float duration);
            void ProcessDown(Transform target, Vector3 scale, float duration);
        }

        public class ResizableView<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
            where T : IPointerProcessor
        {
            [SerializeField] private Transform _target;
            [SerializeField] private Vector3 _pressedScale = new(0.8f, 0.8f, 0.8f);
            [SerializeField] private Vector3 _normalScale = new(1f, 1f, 1f);
            [SerializeField] private float _duration = SharedData.UIResizeSpeed;

            protected T _processor;

            protected virtual void Awake()
            {
                _processor = (T)Activator.CreateInstance(typeof(T));
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                _processor.ProcessUp(_target, _pressedScale, _duration);
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                _processor.ProcessDown(_target, _normalScale, _duration);
            }

            public void OnDrag(PointerEventData eventData)
            {
            }
        }

        public class BaseResizeBehaviour : IPointerProcessor
        {
            protected Tween _downTween;
            protected Tween _upTween;

            public virtual void ProcessUp(Transform target, Vector3 scale, float duration)
            {
                if (_downTween is { active: true })
                    _downTween.Kill();

                _downTween = target.DOScale(scale, duration);
                _downTween.Play();
            }

            public virtual void ProcessDown(Transform target, Vector3 scale, float duration)
            {
                if (_upTween is { active: true })
                    _upTween.Kill();

                _upTween = target.DOScale(scale, duration);
                _upTween.Play();
            }
        }

        public class BasicResizeBehaviour : BaseResizeBehaviour
        {
        }
    }
}