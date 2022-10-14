using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Editor
{
    public class AtlasCreator : ScriptableObject
    {
        [MenuItem("Tools/Create SpriteAtlas")]
        static void Create()
        {
            var atlas = new SpriteAtlas();
            AssetDatabase.CreateAsset(atlas, "Assets/Sprite Atlas.asset");

            Debug.LogError(AssetDatabase.GetAssetPath(atlas));
        }
    }
}