using GameLab;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is used for loading in the Dialogue of room 4. When the loaded in text start with a "Q",
//then it calls the QuestionManager. 
//Usage:
//--------------------------------------------------

public class DialogueManager : Singleton<DialogueManager>
{
	private const int amountOfDialogues = 13;

	public string CurrentDialogue { get; private set; }
	[HideInInspector] public Text dialogueText;
	private int currentDialogueIndex = 0;


	protected override void Awake()
	{
		dialogueText = GetComponentInChildren<Text>();
		Read();
		base.Awake();
	}

	public void Read()
	{
		if (!QuestionManager.Instance.needsAwnser)
		{
			if (currentDialogueIndex <= amountOfDialogues)
			{
				TextAsset loadedDialogue = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DIALOGUE,
					LoadingPaths.FILE_NAME_DIALOGUE) + currentDialogueIndex);
				Write(loadedDialogue);
			}

			currentDialogueIndex++;
		}
	}

	private void Write(TextAsset dialogueToDisplay)
	{
		CurrentDialogue = dialogueToDisplay.ToString();

		if (CurrentDialogue.StartsWith("Q:"))
		{
			QuestionManager.Instance.questionIndex++;
			dialogueText.color = new Color32(48, 178, 156, 255);
			QuestionManager.Instance.needsAwnser = true;
		}
		else
		{
			dialogueText.color = new Color32(40, 40, 40, 255);
		}

		dialogueText.text = CurrentDialogue;
	}

	public void WonMiniGame()
	{
		if (currentDialogueIndex >= amountOfDialogues)
		{
			SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.Medium);
			EventManager.Instance.RaiseEvent(saveItemEvent);

			NextRoomEvent nextRoomEvent = new NextRoomEvent();
			EventManager.Instance.RaiseEvent(nextRoomEvent);
		}
	}
}