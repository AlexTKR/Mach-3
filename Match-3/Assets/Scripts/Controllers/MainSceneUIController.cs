using System.Collections.Generic;
using UnityEngine;
using View;
using Zenject;

namespace Controllers
{
    public interface IMovesCount
    {
        void SetMovesCount(int count);
    }

    public interface IGoal
    {
        void SetGoal(IList<LevelGoal> levelGoals);
    }

    public interface ILevelNumber
    {
        void SetLevelNumber(int number);
    }

    public interface IUpperPanel : IMovesCount, IGoal, ILevelNumber
    {
        
    }

    public class MainSceneUIController : ControllerBase,  IUpperPanel
    {
        private UpperPanel _upperPanel;
        
        
        [Inject]
        void Construct(/*UpperPanel upperPanel */)
        {
            //_upperPanel = upperPanel;
        }

        public void SetMovesCount(int count)
        {
            _upperPanel.SetMovesCount(count);
        }

        public void SetLevelNumber(int number)
        {
            _upperPanel.SetLevelNumber(number);
        }

        public void SetGoal(IList<LevelGoal> levelGoals)
        {
            
        }
    }
}
