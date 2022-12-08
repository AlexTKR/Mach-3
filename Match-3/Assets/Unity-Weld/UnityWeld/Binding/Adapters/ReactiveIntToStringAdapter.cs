using Scripts.Reactive;
using UnityWeld.Binding;

namespace Unity_Weld.UnityWeld.Binding.Adapters
{
    [Adapter(typeof(IReactiveValue<int>), typeof(string))]
    public class ReactiveIntToStringAdapter : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            var value = ((IReactiveValue<int>)valueIn).Value.ToString();
            return value;
        }
    }
}