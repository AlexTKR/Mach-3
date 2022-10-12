
using Settings;
using UnityEngine;

namespace UtilitiesAndHelpers
{
    public static class SharedData
    {
        public static GameSettings GameSettings;
    
        public static float WidthOffset = 0.05f;
        public static float HeightOffset = 0.2f;
        
        public static int MaxRowSize = 8;
        public static int MaxColumnSize = 8;
        public static int CurrentRowCount;
        public static int CurrentColumnCount;

        public static float UIResizeSpeed = 0.4f;
    }
}
