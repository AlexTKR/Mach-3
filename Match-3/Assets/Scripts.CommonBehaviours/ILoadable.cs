using System.Threading.Tasks;

namespace Scripts.CommonBehaviours
{
    public interface ILoadable<T>
    {
        Task<T> Load(bool autoRelease = true, bool runAsync = true);
        void Release();
    }
}