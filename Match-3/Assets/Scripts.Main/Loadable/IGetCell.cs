using Scripts.CommonBehaviours;
using Scripts.Main.View;

namespace Scripts.Main.Loadable
{
    public interface IGetCell
    {
        ILoadable<CellView> GetCellPrefab();
    }
}