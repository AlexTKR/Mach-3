using System.Collections.Generic;
using Scripts.CommonBehaviours;

namespace Scripts.Main.UtilitiesAndHelpers
{
    public enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class Helpers
    {
        public static Dictionary<CellType, string> TypeToName = new Dictionary<CellType, string>()
        {
            { CellType.Yellow, "Yellow" },
            { CellType.Blue, "Blue" },
            { CellType.Green, "Green" },
            { CellType.Red, "Red" },
            { CellType.Purple, "Purple" },
        };
    }
}