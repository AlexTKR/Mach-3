using Settings;

namespace Scripts.Main.UtilitiesAndHelpers
{
    public static class SharedData
    {
        public static int MainSceneIndex = 2;
        public static int MapSceneIndex = 1;

        public static GameSettings GameSettings;
    
        public static float WidthOffset = 0.05f;
        public static float HeightOffset = 0.2f;
        
        public static int MaxRowSize = 8;
        public static int MaxColumnSize = 8;
        public static int CurrentRowCount;
        public static int CurrentColumnCount;

        public static string LastCompletedLevelId = "LastCompletedLevelId";
        public static string SelectedLevel = "SelectedLevel";
    }
}
