using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilitiesAndHelpers;
using Zenject;

namespace Composition
{
    public interface IInit
    {
        Task Initialize();
    }

    public interface IInitControllers : IInit
    {
    }

    public interface IInitView : IInit
    {
    }
    
    public interface IInitBehaviour
    {
        Task InitControllers();
        Task InitViews();
    }

    public class Initer : IInitBehaviour
    {
        private List<IInit> _initControllers;
        private List<IInit> _initViews;
        
        [Inject]
        private void Construct(IList<IInitControllers> initControllers,
            IList<IInitView> initViews)
        {
            _initControllers = initControllers.Select(init => (IInit)init).ToList();
            _initViews = initControllers.Select(init => (IInit)init).ToList();
        }

        public async Task InitControllers()
        {
            for (int i = 0; i < _initControllers.Count; i++)
            {
                await _initControllers[i].Initialize();
            }
        }

        public async Task InitViews()
        {
            for (int i = 0; i < _initViews.Count; i++)
            {
                await _initViews[i].Initialize();
            }
        }
    }
}
