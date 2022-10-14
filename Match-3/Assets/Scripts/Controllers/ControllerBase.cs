using System.Threading.Tasks;
using Composition;
using Zenject;

namespace Controllers
{
    public abstract class ControllerBase : IInitControllers, ITickable
    {
        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }

        public virtual void Tick()
        {
            
        }
    }
}
