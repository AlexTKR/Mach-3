using Scripts.CommonBehaviours;
using Settings;

namespace Scripts.Main.Loadable
{
    public interface IGetLevelSettings
    {
        ILoadable<IGetLevelData> GetLevelSettings();
    }
}