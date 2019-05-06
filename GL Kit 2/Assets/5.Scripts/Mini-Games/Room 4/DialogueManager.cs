using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueManager : MonoBehaviour
{
	#region Varibles
	public Text dialogueText;

	[SerializeField]QuestionManager questionManager;

	private FileStream fileStream;
	private StreamReader reader;
	private string currentDialogue;
	public string CurrentDialogue => currentDialogue;
	private int currentDialogueIndex = -1;
	private int amountOfDialogues = 13;
	#endregion

	private void Awake()
	{
		Read();
	}

	public void Read()
	{		
		currentDialogueIndex++;

		if (currentDialogueIndex <= amountOfDialogues && !questionManager.needsAwnser)
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