using System.Threading.Tasks;
using Composition;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace View
{
    public abstract class PanelBase : MonoBehaviour, IInitView
    {
        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }
    }

    public class UpperPanel : PanelBase
    {
        [SerializeField] private TextMeshProUGUI _movesCount;
        [SerializeField] private TextMeshProUGUI _levelNumber;
        [SerializeField] private MenuButton menuButton;

        public override Task Initialize()
        {
            menuButton.AddOnClickEvent(() =>
            {
                
            });
            return base.Initialize();
        }

        public void SetMovesCount(int count)
        {
            _movesCount.text = count.ToString();
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelNumber.text = $"Level {levelNumber}";
        }
    }
}