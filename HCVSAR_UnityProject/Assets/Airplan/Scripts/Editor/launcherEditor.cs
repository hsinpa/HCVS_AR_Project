using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(launcher))]
[CanEditMultipleObjects]
public class launcherEditor : Editor
{
    SerializedProperty mode;

    SerializedProperty Bullet;

    SerializedProperty angle;
    SerializedProperty ring_quantity;
    SerializedProperty loop_Qt;

    SerializedProperty bout; //執行次數
    SerializedProperty B_CD;//CD時間
    SerializedProperty L_CD; //CD
    SerializedProperty delay;//等待時間

    SerializedProperty replace;
    SerializedProperty initialPosition;
    SerializedProperty initialDirection;
    SerializedProperty rotateCurve;


    void OnEnable()
    {
        // Setup the SerializedProperties.

        mode = serializedObject.FindProperty("Mode");

        Bullet = serializedObject.FindProperty("B_bullet");

        angle = serializedObject.FindProperty("B_angle");
        ring_quantity = serializedObject.FindProperty("B_ring_quantity");
        loop_Qt = serializedObject.FindProperty("B_loop_Qt");
        bout = serializedObject.FindProperty("bout");
        B_CD = serializedObject.FindProperty("B_CD");
        L_CD = serializedObject.FindProperty("L_CD");
        delay = serializedObject.FindProperty("delay");

        replace = serializedObject.FindProperty("replace");
        initialPosition = serializedObject.FindProperty("initialPosition");
        initialDirection = serializedObject.FindProperty("initialDirection");
        rotateCurve = serializedObject.FindProperty("rotateCurve");


    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.PropertyField(mode, new GUIContent("模式"));

        switch (mode.enumValueIndex)
        {
            case (int) ModeState.Create:
                EditorGUILayout.PropertyField(Bullet, new GUIContent("生成物件"));
                EditorGUILayout.PropertyField(loop_Qt, new GUIContent("生成次數"));
                EditorGUILayout.PropertyField(B_CD, new GUIContent("生成間隔"));
                EditorGUILayout.PropertyField(delay, new GUIContent("幾秒後啟動"));
                
                break;
            case (int)ModeState.Ring:
                EditorGUILayout.PropertyField(Bullet, new GUIContent("子彈物件"));

                EditorGUILayout.Slider(angle, 0, 360, new GUIContent("發射擴散角度"));
                EditorGUILayout.PropertyField(ring_quantity, new GUIContent("單次發測量"));

                EditorGUILayout.PropertyField(bout, new GUIContent("每波幾次"));
                EditorGUILayout.PropertyField(B_CD, new GUIContent("每發間隔"));
                EditorGUILayout.PropertyField(L_CD, new GUIContent("每波間隔"));
               

                EditorGUILayout.PropertyField(delay, new GUIContent("幾秒後啟動"));

                break;
            case (int)ModeState.Swirl:
                EditorGUILayout.PropertyField(Bullet, new GUIContent("子彈物件"));

                EditorGUILayout.Slider(angle, 0, 360, new GUIContent("發射擴散角度"));
                EditorGUILayout.PropertyField(ring_quantity, new GUIContent("單次發測量"));

                EditorGUILayout.PropertyField(bout, new GUIContent("每波幾次"));
                EditorGUILayout.PropertyField(B_CD, new GUIContent("每發間隔"));
                EditorGUILayout.PropertyField(L_CD, new GUIContent("每波間隔"));


                EditorGUILayout.PropertyField(delay, new GUIContent("幾秒後啟動"));
                break;
            case (int)ModeState.Line:
                EditorGUILayout.PropertyField(Bullet, new GUIContent("子彈物件"));

                EditorGUILayout.PropertyField(bout, new GUIContent("每波幾次"));
                EditorGUILayout.PropertyField(B_CD, new GUIContent("每發間隔"));
                EditorGUILayout.PropertyField(L_CD, new GUIContent("每波間隔"));


                EditorGUILayout.PropertyField(delay, new GUIContent("幾秒後啟動"));
                break;

        }

        EditorGUILayout.PropertyField(replace, new GUIContent("使用指定路徑"));

        if (replace.boolValue == true)
        {
            EditorGUILayout.PropertyField(initialPosition, new GUIContent("朝向位置"));
            EditorGUILayout.PropertyField(initialDirection, new GUIContent("著陸後方向"));
            EditorGUILayout.PropertyField(rotateCurve, new GUIContent("旋轉曲線"));
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