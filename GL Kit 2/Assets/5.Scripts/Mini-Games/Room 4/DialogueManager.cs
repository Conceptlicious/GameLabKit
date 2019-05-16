﻿using GameLab;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
	private const int amountOfDialogues = 13;

	public string CurrentDialogue { get; private set; }
	[HideInInspector] public Text dialogueText;
	private int currentDialogueIndex = -1;


	protected override void Awake()
	{
		dialogueText = GetComponentInChildren<Text>();
		Read();
		base.Awake();
	}

	public void Read()
	{
		currentDialogueIndex++;

		if (currentDialogueIndex <= amountOfDialogues && !QuestionManager.Instance.needsAwnser)
		{
			TextAsset loadedDialogue = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DIALOGUE,
				LoadingPaths.FILE_NAME_DIALOGUE) + currentDialogueIndex);
			Write(loadedDialogue);
		}
		else
		{
			Debug.LogWarning("Dialogue can not be loaded.");
		}
	}

	private void Write(TextAsset dialogueToDisplay)
	{
		CurrentDialogue = dialogueToDisplay.ToString();

		if (CurrentDialogue.StartsWith("Q:"))
		{
			QuestionManager.Instance.questionIndex++;
			dialogueText.color = Color.cyan;
			QuestionManager.Instance.needsAwnser = true;
		}
		else
		{
			dialogueText.color = Color.white;
		}

		dialogueText.text = CurrentDialogue;
	}
}