using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object to be edited through the DialogueEditor script.
public class DialogueObject : MonoBehaviour
{
	//private string dialogueText = Settings.STR_DEFAULT_DIALOGUE;

	public struct TextInfo
	{
		private string dialogueText;
		public int fileIndex;
		public int containerIndex;
		public string fieldName;
		public int fieldIndex;

		public TextInfo(int fileIndex, int containerIndex, string fieldName, int fieldIndex)
		{
			this.fileIndex = fileIndex;
			this.containerIndex = containerIndex;
			this.fieldName = fieldName;
			this.fieldIndex = fieldIndex;
			dialogueText = Dialogue.GetText(fileIndex, containerIndex, fieldName);
		}

		public string DialogueText
		{
			get
			{
				EventMarkers.Instance.ParseAndCall(dialogueText);
				return dialogueText;
			}
			set
			{
				dialogueText = value;
			}
		}
	}

	private TextInfo info;

	public TextInfo Info
	{
		get => info;
		set
		{
			info = value;
		}
	}

	public string GetNextInContainer()
	{
		int newIndex = 0;
		string str = Dialogue.GetNextText(info.fileIndex, info.containerIndex, info.fieldIndex, out newIndex);
		info.DialogueText = str;
		info.fieldIndex = newIndex;
		return str;
	}

	public string GetTextAt(int pIndex)
	{
		string str = Dialogue.GetTextAt(info.fileIndex, info.containerIndex, pIndex);
		info.DialogueText = str;
		info.fieldIndex = pIndex;
		return str;
	}


	/*public string DialogueText
	{
		get { return dialogueText; }
		set { dialogueText = value; }
	}*/
}
