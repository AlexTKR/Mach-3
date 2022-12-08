using Scripts.Reactive;
using UnityEngine;
using UnityWeld.Binding;

namespace Unity_Weld.UnityWeld.Binding.Adapters
{
    [Adapter(typeof(IReactiveValue<Color>), typeof(Color))]
    public class ReactiveColorToColorAdapter  : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            var value = ((IReactiveValue<Color>)valueIn).Value;
            return value;
        }
    }
}
