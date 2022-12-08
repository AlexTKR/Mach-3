using Scripts.Reactive;
using UnityWeld.Binding;

namespace Unity_Weld.UnityWeld.Binding.Adapters
{
    [Adapter(typeof(IReactiveValue<bool>), typeof(bool))]
    public class ReactiveBoolToBoolAdapter : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            var value = ((IReactiveValue<bool>)valueIn).Value;
            return value;
        }
    }
}
