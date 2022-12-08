using Scripts.CommonBehaviours;
using Scripts.Main.Level;

namespace Scripts.Main.Loadable
{
    public interface IGetLevel
    {
        ILoadable<IGetCells> GetCells();
    }
}