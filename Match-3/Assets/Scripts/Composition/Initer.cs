using System.Collections.Generic;
using System.Linq;
using UtilitiesAndHelpers;
using Zenject;

namespace Composition
{
    public interface IInitBehaviour
    {
        void Init();
    }

    public class Initer : IInitBehaviour
    {
        private List<IInit> _initControllers;
        
        [Inject]
        private void Construct(IList<IInitControllers> initControllers)
        {
            _initControllers = initControllers.Select(init => (IInit)init).ToList();
        }

        public async void Init()
        {
            for (int i = 0; i < _initControllers.Count; i++)
            {
                await _initControllers[i].Initialize();
            }
        }
    }
}
