using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class GoalView : MonoBehaviour
    {
        [SerializeField] private Image _goalImage;
        [SerializeField] private TextMeshProUGUI _goalCount;

        public void SetImage(Sprite sprite)
        {
            _goalImage.sprite = sprite;
        }

        public void SetCount(int count)
        {
            _goalCount.text = count.ToString();
        }
    }
}
