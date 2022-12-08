using System.Collections.Generic;
using Scripts.CommonExtensions.Scripts;
using Scripts.Main.Controllers;

namespace Scripts.Main.UtilitiesAndHelpers
{
    public interface IPool<T>
    {
        T Get();
        void Return(T view);
    }

    public class CellPool<T> : IPool<T> where T : ICell
    {
        private List<T> _pooled;
        private IFactory<ICell> _factory;


        public CellPool(IFactory<ICell> factory)
        {
            _pooled = new List<T>();
            _factory = factory;
        }

        public T Get()
        {
            T element;
            if (_pooled.Count > 0)
                element = _pooled.ReturnAndRemoveAt(0);
            else
                element = (T)_factory.Get();
            
            return element;
        }
        
        public void Return(T view)
        {
            _pooled.Add(view);
        }
    }
}