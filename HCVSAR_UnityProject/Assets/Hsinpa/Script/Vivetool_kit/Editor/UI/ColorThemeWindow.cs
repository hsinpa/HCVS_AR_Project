// ========================================================================== //
//
//  class ColorThemeWindow
//  -----
//  Purpose: Color Theme Editor window
//
//
//  Created: 2017-04-04
//  Updated: 2017-04-04
//
//  Copyright 2017 Yu-hsien Chang
// 
// ========================================================================== //
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Linq;

namespace Htc.ViveToolkit.UI
{
    public class ColorThemeWindow : EditorWindow
    {

        Vector2 scrollPos;

        private static ColorThemeData _data;
        private static ColorThemeData data
        {
            get
            {
                if (_data == null)
                    _data = AssetUtility.Load<ColorThemeData>(ColorThemeData.AssetName);
                return _data;
            }

            set { _data = value; }
        }

        public static bool GetColor(string name, out Color color)
        {
            color = Color.white;

            if (data == null)
                return false;

            var namedColor = data.colors.Find((c) => c.name == name);
            if (namedColor == null)
                return false;

            color = namedColor.color;

            return true;
        }

        public static bool GetColorBlock(string name, out ColorBlock colorBlock)
        {
            colorBlock = ColorBlock.defaultColorBlock;

            if (data == null)
                return false;

            var namedColorBlock = data.colorBlocks.Find((c) => c.name == name);
            if (namedColorBlock == null)
                return false;

            colorBlock = namedColorBlock.colorBlock;

            return true;
        }

        private static string[] _colorNames;
        public static string[] colorNames
        {
            get
            {
                if (_colorNames == null)
                    UpdateNames();

                return _colorNames;
            }

            private set { _colorNames = value; }
        }

        private static string[] _colorBlockNames;
        public static string[] colorBlockNames
        {
            get
            {
                if (_colorBlockNames == null)
                    UpdateNames();

                return _colorBlockNames;
            }

            private set { _colorBlockNames = value; }
        }

        private static void UpdateNames()
        {
            if (data == null)
            {
                colorNames = null;
                colorBlockNames = null;
                return;
            }

            colorNames = new string[data.colors.Count];

            var i = 0;
            foreach (var color in data.colors)
                colorNames[i++] = color.name;

            colorBlockNames = new string[data.colorBlocks.Count];

            i = 0;
            foreach (var colorBlock in data.colorBlocks)
                colorBlockNames[i++] = colorBlock.name;
        }

        SerializedObject obj;

        private ReorderableList colorList;
        private ReorderableList colorBlockList;

        [MenuItem("ViveToolkit/Color Theme Editor")]
        public static void Init()
        {
            var window = GetWindow<ColorThemeWindow>(false, "Color Theme", true);
            window.position = new Rect(40, 40, 200, 400);
        }

        private void OnEnable()
        {
            // Load data from asset
            if (data == null)
                data = AssetUtility.Load<ColorThemeData>(ColorThemeData.AssetName, true);

            obj = new SerializedObject(data);

            colorList = new ReorderableList(obj, obj.FindProperty("colors"));
            colorList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Colors"); };
            colorList.elementHeight = EditorGUIUtility.singleLineHeight;
            colorList.drawElementCallback = OnDrawColorElement;

            colorBlockList = new ReorderableList(obj, obj.FindProperty("colorBlocks"));
            colorBlockList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Color Blocks"); };
            colorBlockList.elementHeightCallback = GetColorBlockElementHeight;
            colorBlockList.drawElementCallback = OnDrawColorBlockElement;
        }


        private void OnGUI()
        {
            obj.Update();

            EditorGUILayout.Space();
            colorList.DoLayoutList();
            EditorGUILayout.Space();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MinWidth(500), GUILayout.MinWidth(900));

                colorBlockList.DoLayoutList();

            EditorGUILayout.EndScrollView();


            obj.ApplyModifiedProperties();

            if(GUI.changed)
            {
                var items = FindObjectsOfType<ColorThemeElement>();
                foreach (var item in items)
                    ColorThemeElementEditor.UpdateColor(item);
            }

            UpdateNames();
        }

        private void OnDrawColorElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var color = colorList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight), color.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight), color.FindPropertyRelative("color"), GUIContent.none);
        }

        private float GetColorBlockElementHeight(int index)
        {
            var colorBlock = colorBlockList.serializedProperty.GetArrayElementAtIndex(index);
            var drawer = new ColorBlockDrawer();
            return drawer.GetPropertyHeight(colorBlock, new GUIContent(string.Empty)) + EditorGUIUtility.singleLineHeight * 1.5f;
        }

        private void OnDrawColorBlockElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var colorBlock = colorBlockList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), colorBlock.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), colorBlock.FindPropertyRelative("colorBlock"), GUIContent.none);
        }
    }
}