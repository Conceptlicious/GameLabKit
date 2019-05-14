using GameLab;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
	private FileStream fileStream;
	private StreamReader reader;

	private const int amountOfDialogues = 13;

	public string CurrentDialogue { get; private set; }
	[HideInInspector] public Text dialogueText;
	private int currentDialogueIndex = -1;
	

	protected override void Awake()
	{
		base.Awake();
		Read();
	}

	public void Read()
	{		
		currentDialogueIndex++;

		if (currentDialogueIndex <= amountOfDialogues && !QuestionManager.Instance.needsAwnser)
		{
			fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DIALOGUE, LoadingPaths.FILE_NAME_DIALOGUE)
				+ currentDialogueIndex + LoadingPaths.FILE_TYPE);
			reader = new StreamReader(fileStream);

			using (fileStream) using (reader)
			{
				CurrentDialogue = reader.ReadToEnd();
				Write(CurrentDialogue);
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
			QuestionManager.Instance.questionIndex++;
			dialogueText.color = Color.cyan;
			QuestionManager.Instance.needsAwnser = true;
		}
		else
		{
			dialogueText.color = Color.white;
		}

		dialogueText.text = displayedDialogue;
	}
}