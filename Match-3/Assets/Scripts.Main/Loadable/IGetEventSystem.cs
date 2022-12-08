using Scripts.CommonBehaviours;
using UnityEngine.EventSystems;

namespace Scripts.Main.Loadable
{
    public interface IGetEventSystem
    {
        ILoadable<EventSystem> GetEventSystem();
    }
}