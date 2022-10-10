using System.Collections.Generic;
using UnityEngine;

namespace UtilitiesAndHelpers
{
    public static class Extensions
    {
        public static int GetRowCount<T>(this T[,] collection)
        {
            return collection is null ? 0 : collection.GetUpperBound(0) + 1;
        }

        public static int GetColumnCount<T>(this T[,] collection)
        {
            return collection is null ? 0 : collection.GetUpperBound(1) + 1;
        }

        public static void SetActiveOptimize(this GameObject gameObject, bool status)
        {
            if (gameObject.activeSelf != status)
                gameObject.SetActive(status);
        }

        public static T ReturnAndRemoveAt<T>(this List<T> collection, int index)
        {
            var element = collection[index];
            collection.RemoveAt(index);
            return element;
        }
    }
}