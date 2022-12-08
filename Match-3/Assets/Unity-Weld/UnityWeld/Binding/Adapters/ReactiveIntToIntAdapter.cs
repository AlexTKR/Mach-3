using Scripts.Reactive;
using UnityWeld.Binding;

namespace Unity_Weld.UnityWeld.Binding.Adapters
{
    [Adapter(typeof(IReactiveValue<int>), typeof(int))]
    public class ReactiveIntToIntAdapter : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            var value = ((IReactiveValue<int>)valueIn).Value;
            return value;
        }
    }
}
