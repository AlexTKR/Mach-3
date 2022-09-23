using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public interface IGetLevel
    {
        CellType[,] GetLevel(int number);
    }

    [CreateAssetMenu(menuName = "Holders/LevelHolder", fileName = "LevelHolder")]
    public class LevelHolder : ScriptableObject, IGetLevel
    {
        public List<LevelInstance> levels;
        
        public CellType[,] GetLevel(int number)
        {
            return levels.Count < number ? null : levels[number - 1].CellTypes;
        }
    }
}
