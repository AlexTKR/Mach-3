using System.Collections.Generic;
using Scripts.CommonBehaviours;
using UnityEngine;

namespace Scripts.Main.Level
{
    public interface IGetCells
    {
        CellType[,] GetCells(int number);
    }

    [CreateAssetMenu(menuName = "Holders/LevelHolder", fileName = "LevelHolder")]
    public class LevelHolder : ScriptableObject, IGetCells
    {
        public List<LevelInstance> levels;
        
        public CellType[,] GetCells(int number)
        {
            return levels.Count < number ? null : levels[number - 1].CellTypes;
        }
    }
}
