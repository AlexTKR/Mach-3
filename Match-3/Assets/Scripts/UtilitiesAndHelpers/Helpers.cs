using System.Collections.Generic;
using System.Threading.Tasks;
using Level;

namespace UtilitiesAndHelpers
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
        public static Dictionary<CellType, string> CellTypeToName = new Dictionary<CellType, string>()
        {
            { CellType.Yellow, "Yellow" },
            { CellType.Blue, "Blue" },
            { CellType.Green, "Green" },
            { CellType.Red, "Red" },
            { CellType.Purple, "Purple" },
        };
    }
}