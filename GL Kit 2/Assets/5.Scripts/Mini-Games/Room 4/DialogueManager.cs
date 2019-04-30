﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueManager : MonoBehaviour
{
	#region Varibles
	public Text dialogueText;

	[SerializeField]QuestionManager questionManager;

	FileStream fileStream;
	StreamReader reader;
	string currentDialogue;
	int currentDialogueIndex = -1;
	#endregion

	private void Awake()
	{
		Read();
	}

	public void Read()
	{		
		currentDialogueIndex++;

		if (currentDialogueIndex <= 13 && !questionManager.needsAwnser)
		{
			fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DIALOGUE, LoadingPaths.FILE_NAME_DIALOGUE) + currentDialogueIndex + LoadingPaths.FILE_TYPE);
			reader = new StreamReader(fileStream);

			using (fileStream) using (reader)
			{
				currentDialogue = reader.ReadToEnd();
				Write(currentDialogue);
			}
		}
		else
		{
			Debug.LogWarning("Dialogue can not be loaded.");
		}
	}

	private void Write(string displayedDialogue)
	{
		if (displayedDialogue.StartsWith("Q:"))
		{
			questionManager.questionIndex++;
			dialogueText.color = Color.cyan;
			questionManager.needsAwnser = true;
		}
		else
		{
			dialogueText.color = Color.white;
		}

		dialogueText.text = displayedDialogue;
	}
}