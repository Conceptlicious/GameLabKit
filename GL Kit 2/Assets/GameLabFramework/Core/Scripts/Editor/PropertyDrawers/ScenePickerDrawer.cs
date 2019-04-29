using System;
using UnityEditor;
using UnityEngine;

namespace GameLab
{
	[CustomPropertyDrawer(typeof(ScenePickerAttribute), true)]
	public class ScenePickerDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SceneAsset currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

			EditorGUI.BeginChangeCheck();
			SceneAsset newScene = EditorGUI.ObjectField(position, label, currentScene, typeof(SceneAsset), false) as SceneAsset;

			if(EditorGUI.EndChangeCheck())
			{
				property.stringValue = AssetDatabase.GetAssetPath(newScene);
			}
		}
	}
}
