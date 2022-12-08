using System.Threading.Tasks;
using Scripts.CommonBehaviours;
using Zenject;

namespace Scripts.Main
{
    public interface IInitiator : IInit
    {
        Task Init();
    }

    public class Initiator : IInitiator
    {
        private IInit[] _initControllers;

        [Inject]
        private void Construct(IInit[] initControllers)
        {
            _initControllers = initControllers;
        }

        public async Task Init()
        {
            for (int i = 0; i < _initControllers.Length; i++)
            {
                await _initControllers[i].Init();
            }
        }
    }
}