using Scripts.Reactive;
using UnityWeld.Binding;

namespace Unity_Weld.UnityWeld.Binding.Adapters
{
    [Adapter(typeof(IReactiveValue<string>), typeof(string))]
    public class ReactiveStringToStringAdapter : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            var value = ((IReactiveValue<string>)valueIn);
            return value.Value;
        }
    }
}