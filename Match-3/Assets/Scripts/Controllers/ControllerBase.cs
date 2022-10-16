using System.Threading.Tasks;
using Composition;
using DB;
using Zenject;

namespace Controllers
{
    public abstract class ControllerBase : IInitControllers, ITickable,
        IDatabaseUser
    {
        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }
        
        public virtual Task InjectDatabase(IDatabase database)
        {
            return Task.CompletedTask;
        }

        public virtual void Tick()
        {
            
        }
    }
}
