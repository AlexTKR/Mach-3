using System.Threading.Tasks;
using DB;
using Scripts.CommonBehaviours;
using Scripts.Main.DB;
using Zenject;

namespace Scripts.Main.Controllers
{
    public abstract class ControllerBase :
        IDatabaseUser, IInit, ITickable
    {
        public virtual Task InjectDatabase(IDatabase database)
        {
            return Task.CompletedTask;
        }

        public virtual void Tick()
        {
            
        }

        public  virtual  Task Init()
        {
            return Task.CompletedTask;
        }
    }
}
