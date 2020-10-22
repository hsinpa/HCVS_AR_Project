﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceGraphicsToolkit
{
	/// <summary>This attribute allows you to turn a normal integer into a seed integer, adding a button to its inspector, allowing you to quickly randomize it.</summary>
	public class SgtSeedAttribute : PropertyAttribute
	{
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	[CustomPropertyDrawer(typeof(SgtSeedAttribute))]
	public class SgtSeedDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var rect1 = position; rect1.xMax = position.xMax - 20;
			var rect2 = position; rect2.xMin = position.xMax - 18;

			EditorGUI.PropertyField(rect1, property, label);

			if (GUI.Button(rect2, "R") == true)
			{
				var path    = property.propertyPath;
				var objects = property.serializedObject.targetObjects;
				var context = property.serializedObject.context;

				for (var i = objects.Length - 1; i >= 0; i--)
				{
					var obj = new SerializedObject(objects[i], context);
					var pro = obj.FindProperty(path);

					pro.intValue = Random.Range(int.MinValue, int.MaxValue);

					obj.ApplyModifiedProperties();
				}
			}
		}
	}
}
#endif