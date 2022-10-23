using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public AnimationCurve ShiftCellCurve;
        public float CellShiftSpeed;
        public int SpawnDelay;
    }
}
