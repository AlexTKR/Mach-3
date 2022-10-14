using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;

namespace Settings
{
    public interface IGetLevelData
    {
        List<LevelData> LevelData {get;}
    }

    [CreateAssetMenu(menuName = "Settings/LevelSettings", fileName = "LevelSettings")]
    public class LevelSettings :ScriptableObject, IGetLevelData 
    {
        [SerializeField] private List<LevelData> _levelData;

        public List<LevelData> LevelData => _levelData.OrderBy(data => data.LevelNumber).ToList();
    }
}
