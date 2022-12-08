using System;
using Scripts.CommonBehaviours;
using Scripts.Main.Level;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelConstructor))]
    public class LevelEditor : UnityEditor.Editor
    {
        private LevelInstance _currentLevelInstance;
        private LevelConstructor _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _target = target as LevelConstructor;

            if (_target?.currentLevelInstance is null)
            {
                EditorGUILayout.LabelField("Current level is null select level to work on");
                return;
            }

            if (GUILayout.Button("Select current level", GUILayout.ExpandWidth(true)))
            {
                LoadLevel();
            }
            
            if (GUILayout.Button("Reset current level", GUILayout.ExpandWidth(true)))
            {
                SetLevelDataToDefault();
            }


            if (_target.currentLevelInstance == null || _target.currentLevelInstance.CellTypes == null)
            {
                EditorGUILayout.LabelField("Press select current level to continue");
                return;
            }
            
            DisplayLevel();
        }

        private void LoadLevel()
        {
            _currentLevelInstance = _target.currentLevelInstance;
            var currCells = _currentLevelInstance.CellTypes;
            
             if (currCells is null)
             {
                 SetLevelDataToDefault();
             }
        }

        private void SetLevelDataToDefault()
        {
            var x = _target.Dimensions.x;
            var y = _target.Dimensions.y;
            var cells = new CellType[x, y];
            
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    cells[i, j] = CellType.Empty;
                }
            }

            SaveCells(cells);
        }

        private void SaveCells(CellType[,] cells)
        {
            _target.currentLevelInstance.CellTypes = cells;
            EditorUtility.SetDirty(_target.currentLevelInstance);
        }

        private void DisplayLevel()
        {
            var cells = _target.currentLevelInstance.CellTypes;
            int rows = cells.GetUpperBound(0);
            int columns = cells.GetUpperBound(1);
            bool hasChanged = false;
            
            var tableStyle = new GUIStyle ("box")
            {
                padding = new RectOffset (10, 10, 10, 10),
            };
            
            var columnStyle = new GUIStyle
            {
                fixedWidth = 80
            };
            
            EditorGUILayout.BeginHorizontal (tableStyle);
            for (int x = 0; x <= rows; x++) 
            {
                EditorGUILayout.BeginVertical (columnStyle);
                for (int y = 0; y <= columns; y++) 
                {
                    var currLevelValue = (CellType)EditorGUILayout.EnumPopup(cells[x,y]);
                    if (cells[x, y] != currLevelValue)
                    {
                        cells[x, y] = currLevelValue;
                        hasChanged = true;
                    }
                    
                }
            
                EditorGUILayout.EndVertical ();
            }
            
            EditorGUILayout.EndHorizontal ();
            
            if (hasChanged)
            {
                SaveCells(cells);
            }
        }
    }
}