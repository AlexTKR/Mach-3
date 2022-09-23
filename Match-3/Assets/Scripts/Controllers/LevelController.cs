using System;
using System.Threading.Tasks;
using Level;
using UnityEngine;
using UnityEngine.U2D;
using UtilitiesAndHelpers;
using View;
using Zenject;

namespace Controllers
{
    public class LevelData
    {
        public int LevelNumber;
        public CellType[,] Cells;
    }

    public interface ILoadLevel
    {
        void LoadLevel(LevelData levelData);
    }

    public class LevelController : ControllerBase, ILoadLevel
    {
        [Inject]
        void Construct()
        {
            
        }

        public override void Tick()
        {
        }

        public void LoadLevel(LevelData levelData)
        {
        }
    }
}