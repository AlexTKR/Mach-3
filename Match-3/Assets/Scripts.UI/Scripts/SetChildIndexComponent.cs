using UnityEngine;

namespace Scripts.UI.Scripts
{
    public class SetChildIndexComponent : MonoBehaviour
    {
        public int SetSiblingIndex
        {
            get => transform.GetSiblingIndex();
            set => transform.SetSiblingIndex(value);
        }
    }
}