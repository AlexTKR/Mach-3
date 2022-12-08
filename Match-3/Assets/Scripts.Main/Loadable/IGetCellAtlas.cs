using Scripts.CommonBehaviours;
using UnityEngine.U2D;

namespace Scripts.Main.Loadable
{
    public interface IGetCellAtlas
    {
        ILoadable<SpriteAtlas> GetCellAtlas();
    }
}