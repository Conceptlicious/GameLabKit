using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Helper script for the custom Unity Editor, allowing each object to have specific dialogue assigned to it.
/// </summary>
[CustomEditor(typeof(DialogueObject))]
public class DialogueEditor : Editor
{

	private string[] fileNames;
	private int fileNameIndex = 0;
	private int fileNameIndexPrevious = 0;

	private string[] containerNames;
	private int containerNameIndex = 0;
	private int containerNameIndexPrevious = 0;

	private string[] keyNames;
	private int keyNameIndex = 0;

	public override void OnInspectorGUI()
	{
		if (DrawDefaultInspector())
		{
			//if is auto
			//do thing

		}

		//if (GameData.Initialised)
		//{
			PopulateOptions();
		//}

		
	}


	private void LoadExistingValues(DialogueObject pObject)
	{
		fileNameIndex = pObject.Info.fileIndex;
		containerNameIndex = pObject.Info.containerIndex;
		keyNameIndex = pObject.Info.fieldIndex;
	}

	private void PopulateOptions()
	{
		DialogueObject dialogueObject = (DialogueObject)target;
		LoadExistingValues(dialogueObject);

		if (GameData.Initialised == false)
		{
			Dialogue.LoadAllText();
			GameData.Initialised = true;
		}
		
		fileNames = Dialogue.GetFileNames();
		fileNameIndexPrevious = fileNameIndex;
		fileNameIndex = EditorGUILayout.Popup(fileNameIndex, fileNames);

		//Reset indices if options change.
		if (fileNameIndexPrevious != fileNameIndex)
		{
			Debug.Log("Resetting");
			containerNameIndex = 0;
			keyNameIndex = 0;
		}

		containerNames = Dialogue.GetContainerNames(fileNameIndex);
		containerNameIndexPrevious = containerNameIndex;
		containerNameIndex = EditorGUILayout.Popup(containerNameIndex, containerNames);

		//Reset indices if options change.
		if (containerNameIndexPrevious != containerNameIndex)
		{
			Debug.Log("Resetting key index");
			keyNameIndex = 0;
		}

		keyNames = Dialogue.GetKeyNames(fileNameIndex, containerNameIndex);
		keyNameIndex = EditorGUILayout.Popup(keyNameIndex, keyNames);

		// DialogueObject.TextInfo ti = new DialogueObject.TextInfo(fileNameIndex, containerNameIndex, keyNames[keyNameIndex], keyNameIndex);
		dialogueObject.Info = new DialogueObject.TextInfo(fileNameIndex, containerNameIndex, keyNames[keyNameIndex], keyNameIndex);
		if (GUILayout.Button("Print Dialogue"))
		{
			Debug.Log(dialogueObject.Info.DialogueText);
		}
		if (GUILayout.Button("Progress Dialogue"))
		{
			Debug.Log(dialogueObject.GetTextAndIterate());
			keyNameIndex = dialogueObject.Info.fieldIndex;
		}

		
		serializedObject.ApplyModifiedProperties();
		EditorUtility.SetDirty(target);
	}
}
