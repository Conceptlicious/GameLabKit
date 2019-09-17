using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object to be edited through the DialogueEditor script.
public class DialogueObject : MonoBehaviour
{
	//private string dialogueText = Settings.STR_DEFAULT_DIALOGUE;

	[Serializable]
	public struct TextInfo
	{
		private string dialogueText;
		public int fileIndex;
		public int containerIndex;
		public string fieldName;
		public int fieldIndex;
		public bool completeReading;

		
		public TextInfo(int pFileIndex, int pContainerIndex, string pFieldName, int pFieldIndex)
		{
			fileIndex = pFileIndex;
			containerIndex = pContainerIndex;
			fieldName = pFieldName;
			fieldIndex = pFieldIndex;
			dialogueText = Dialogue.GetText(fileIndex, containerIndex, fieldName);
			completeReading = false;
		}
		
		public string DialogueText
		{
			get
			{
				dialogueText = EventMarkers.Instance.ParseAndCall(dialogueText);
				return dialogueText;
			}
			set
			{
				dialogueText = value;
			}
		}
	}

	[SerializeField, HideInInspector]
	private TextInfo info;

	public TextInfo Info
	{
		get => info;
		set => info = value;
	}

	public string GetTextAndIterate()
	{
		info.DialogueText = Dialogue.GetTextAndIterate(info.fileIndex, info.containerIndex, info.fieldIndex, out int newIndex, out bool completeReading);
		info.fieldIndex = newIndex;
		info.completeReading = completeReading;
		Debug.Log("New Field Index: " + info.fieldIndex);
		return info.DialogueText;
	}

	public string GetText()
	{
		return info.DialogueText;
	}

	public string GetTextAt(int pIndex)
	{
		info.DialogueText = Dialogue.GetTextAt(info.fileIndex, info.containerIndex, pIndex);
		info.fieldIndex = pIndex;
		return info.DialogueText;
	}

	public string GetRandomText()
	{
		string str = Dialogue.GetRandomText(info.fileIndex, info.containerIndex);
		info.DialogueText = str;
		return info.DialogueText;
	}

	public string GetFileName()
	{
		return Dialogue.GetFileNames()[info.fileIndex];
	}

	public int GetMaxFieldIndices()
	{
		return Dialogue.GetMaxFieldIndices(info.fileIndex, info.containerIndex);
	}

	public string GetContainerName()
	{
		string[] strArr = Dialogue.GetContainerNames(info.fileIndex);
		return strArr[info.containerIndex];
	}
	
	}
	