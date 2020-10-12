using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(EnemyGeneral))]
[CanEditMultipleObjects]
public class EnemyGeneralEditor : Editor
{
    SerializedProperty mode;


    SerializedProperty Speed;
    SerializedProperty rSpeed;
    SerializedProperty HP;
    SerializedProperty DeadEffect;
    SerializedProperty initialPosition;
    SerializedProperty initialDirection;
    SerializedProperty rotateCurve;

    SerializedProperty keyOrder;

    void OnEnable()
    {
        // Setup the SerializedProperties.

        mode = serializedObject.FindProperty("Mods");

        Speed = serializedObject.FindProperty("speed");
        rSpeed = serializedObject.FindProperty("rSpeed");
        HP = serializedObject.FindProperty("HP");
        DeadEffect = serializedObject.FindProperty("DeadEffect");

        initialPosition = serializedObject.FindProperty("initialPosition");
        initialDirection = serializedObject.FindProperty("initialDirection");
        rotateCurve = serializedObject.FindProperty("rotateCurve");
        keyOrder = serializedObject.FindProperty("keyOrder");

    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.PropertyField(mode, new GUIContent("模式"));

        switch (mode.enumValueIndex)
        {
            case (int)EnemyMode.Wander:
                EditorGUILayout.PropertyField(Speed, new GUIContent("速度"));
                EditorGUILayout.PropertyField(rSpeed, new GUIContent("轉向速度"));
                EditorGUILayout.PropertyField(HP, new GUIContent("生命值"));
                EditorGUILayout.PropertyField(DeadEffect, new GUIContent("被破壞特效"),true);
                EditorGUILayout.PropertyField(initialPosition, new GUIContent("朝向位置"));
                EditorGUILayout.PropertyField(initialDirection, new GUIContent("著陸後方向"));
                EditorGUILayout.PropertyField(rotateCurve, new GUIContent("旋轉曲線"));
                break;
            case (int)EnemyMode.follow:
                EditorGUILayout.PropertyField(Speed, new GUIContent("速度"));
                EditorGUILayout.PropertyField(rSpeed, new GUIContent("轉向速度"));
                EditorGUILayout.PropertyField(HP, new GUIContent("生命值"));
                EditorGUILayout.PropertyField(DeadEffect, new GUIContent("被破壞特效"), true);
                break;
            case (int)EnemyMode.Snake:
                EditorGUILayout.PropertyField(Speed, new GUIContent("速度"));
                EditorGUILayout.PropertyField(rSpeed, new GUIContent("轉向速度"));
                EditorGUILayout.PropertyField(HP, new GUIContent("生命值"));
                EditorGUILayout.PropertyField(DeadEffect, new GUIContent("被破壞特效"), true);
                break;
           

        }

    
        

        serializedObject.ApplyModifiedProperties();
    }

    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}