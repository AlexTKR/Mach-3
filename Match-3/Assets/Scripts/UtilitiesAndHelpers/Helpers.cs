using System.Collections.Generic;
using System.Threading.Tasks;
using Level;

namespace UtilitiesAndHelpers
{
    #region Interfaces

    public interface IInit
    {
        Task Initialize();
    }

    public interface IInitControllers : IInit
    {
    }

    public interface IInitView : IInit
    {
    }

    #endregion

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