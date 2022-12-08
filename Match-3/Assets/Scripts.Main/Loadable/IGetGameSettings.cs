using Scripts.CommonBehaviours;
using Settings;

namespace Scripts.Main.Loadable
{
    public interface IGetGameSettings
    {
        ILoadable<GameSettings> GetGameSettings();
    }
}