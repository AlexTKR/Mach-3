namespace Scripts.Main.UtilitiesAndHelpers
{
    public static class Extensions
    {
        public static int GetRowCount<T>(this T[,] collection)
        {
            return collection is null ? 0 : collection.GetUpperBound(0) + 1;
        }
    }
}