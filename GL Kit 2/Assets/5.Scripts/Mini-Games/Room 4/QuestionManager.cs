using GameLab;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script handles the Questions and the answers that are given. It is called from the Dialogue-
//Manager when a question is in the dialogue.
//Usage: Once in room 4 on the Dialogue/QuestionManager Object.
//--------------------------------------------------

public class QuestionManager : Singleton<QuestionManager>
{
	private const string WRONG_ANSWER_MESSAGE = "That is not the right awnser.\nPlease try again.";
	private const float TIME_WRONG_MESSAGE_DISPLAYED = 3f;

	[HideInInspector] public bool needsAwnser = false;
	[HideInInspector] public int questionIndex = -1;
	private Dictionary<int, int> rightAwners = new Dictionary<int, int>();

	protected override void Awake()
	{
		base.Awake();
		SetVariables();
	}

	public void CheckAwnser()
	{
		if (needsAwnser)
		{
			int awnser = ConveyorBeltMovement.Instance.CurrentPlatformIndex;

			if (awnser == rightAwners[questionIndex])
			{
				needsAwnser = false;
				DialogueManager.Instance.Read();
			}
			else
			{
				StartCoroutine(DisplayWrongAwnserMessage(DialogueManager.Instance.CurrentDialogue));
			}
		}
	}

	private IEnumerator DisplayWrongAwnserMessage(string lastQuestion)
	{
		DialogueManager.Instance.dialogueText.color = Color.red;
		DialogueManager.Instance.dialogueText.text = WRONG_ANSWER_MESSAGE;

		yield return new WaitForSeconds(TIME_WRONG_MESSAGE_DISPLAYED);

		DialogueManager.Instance.dialogueText.color = new Color32(48, 178, 156, 255);
		DialogueManager.Instance.dialogueText.text = lastQuestion;

	}

	private void SetVariables()
	{
		rightAwners.Add(0, (int)PlatformType.Exergames);
		rightAwners.Add(1, (int)PlatformType.VirtualReality);
		rightAwners.Add(2, (int)PlatformType.AugmentedReality);
		rightAwners.Add(3, (int)PlatformType.Console);
		rightAwners.Add(4, (int)PlatformType.Wearables);
	}
}