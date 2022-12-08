using System;

namespace Scripts.Reactive
{
    public interface IReactiveValue<T>
    {
        T Value { get; set; }
        void Destroy();
        event Action OnChanged;
    }

    public class ReactiveValue<T> : IReactiveValue<T>
    {
        private T _value;
        private bool _initialSet;

        public event Action OnChanged;

        public void Destroy()
        {
            OnChanged = null;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value) && _initialSet)
                    return;

                _value = value;
                _initialSet = true;
                OnChanged?.Invoke();
            }
        }
    }

    public class ReactiveString : IReactiveValue<string>
    {
        private string _value;
        private bool _initialSet;
        private string _format;

        public event Action OnChanged;

        public ReactiveString(string format)
        {
            _format = format;
        }

        public void Destroy()
        {
            OnChanged = null;
        }

        public string Value
        {
            get
            {
                var str = string.Format(_format, _value);
                return str;
            }
            set
            {
                if (_value is { } && _value.Equals(value) && _initialSet)
                    return;

                _value = value;
                _initialSet = true;
                OnChanged?.Invoke();
            }
        }
    }
}