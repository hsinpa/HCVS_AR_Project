// ========================================================================== //
//
//  class ColorThemeElementEditor
//  -----
//  Purpose: Custom editor for ColorThemeElementEditor
//
//
//  Created: 2017-04-04
//  Updated: 2017-04-10
//
//  Copyright 2017 Yu-hsien Chang
// 
// ========================================================================== //
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Htc.ViveToolkit.UI
{
    [CustomEditor(typeof(ColorThemeElement))]
    public class ColorThemeElementEditor : Editor
    {
        private SerializedProperty controlTarget;
        private SerializedProperty colorName;

        private bool enabled = false;

        void OnEnable()
        {
            controlTarget = serializedObject.FindProperty("_target");
            colorName = serializedObject.FindProperty("_colorName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = ((MonoBehaviour)target).enabled;

            string[] names = null;

            // Get the list of names
            switch ((ColorThemeElement.Target)controlTarget.enumValueIndex)
            {
                case ColorThemeElement.Target.Graphic:
                    names = ColorThemeWindow.colorNames;
                    break;

                case ColorThemeElement.Target.Selectable:
                    names = ColorThemeWindow.colorBlockNames;
                    break;
            }

            if (names != null || names.Length == 0)
            {
                // Find the index of currently selected names
                var index = ArrayUtility.IndexOf(names, colorName.stringValue);

                if (index >= 0)
                {
                    EditorGUILayout.PropertyField(controlTarget);
                    var newIndex = EditorGUILayout.Popup("Color", index, names);
                    if (newIndex != index)
                    {
                        colorName.stringValue = names[newIndex];
                        //UpdateColor((ColorTheme)target);
                    }
                }
                else
                {
                    // Show error message
                    var curName = colorName.stringValue;
                    if (string.IsNullOrEmpty(curName))
                    {
                        EditorGUILayout.HelpBox("Please select a color", MessageType.None);
                        curName = "(None)";
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("The color \"" + colorName.stringValue + "\" was not found", MessageType.Warning);
                    }

                    EditorGUILayout.PropertyField(controlTarget);

                    var newNames = new string[names.Length + 1];
                    newNames[0] = curName;
                    names.CopyTo(newNames, 1);

                    var newIndex = EditorGUILayout.Popup("Color", 0, newNames);
                    if (newIndex != 0)
                    {
                        colorName.stringValue = newNames[newIndex];
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No color available. Please add colors from the Color Theme Editor.", MessageType.None);
            }

            // Force update if just enabled
            if (!enabled && GUI.enabled)
                UpdateColor((ColorThemeElement)target);

            enabled = GUI.enabled;
            GUI.enabled = true;

            EditorGUILayout.Space();
            if (GUILayout.Button("Open Color Theme Editor"))
                ColorThemeWindow.Init();

            serializedObject.ApplyModifiedProperties();

            if(GUI.changed)
                UpdateColor(target as ColorThemeElement);
        }

        public static void UpdateColor(ColorThemeElement item)
        {
            Undo.RecordObject(item, "Changed color");

            switch (item.target)
            {
                case ColorThemeElement.Target.Graphic:
                    Color color;
                    if (ColorThemeWindow.GetColor(item.colorName, out color))
                    {
                        item.color = color;
                        item.Refresh();
                    }
                    break;

                case ColorThemeElement.Target.Selectable:
                    ColorBlock colorBlock;
                    if (ColorThemeWindow.GetColorBlock(item.colorName, out colorBlock))
                    {
                        item.colorBlock = colorBlock;
                        item.Refresh();
                        EditorUtility.SetDirty(item);
                    }
                    break;
            }
        }
    }
}