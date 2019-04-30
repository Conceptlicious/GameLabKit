using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
	#region Varibles
	private const string wrongAwnserMessage = "That is not the right awnser.\nPlease try again.";

	[HideInInspector] public bool needsAwnser = false;
	[HideInInspector] public int questionIndex = -1;

	[SerializeField] DialogueManager dialogueManager;
	[SerializeField] ConveyorBeltMovement conveyorBeltMovement;

	Dictionary<int, int> rightAwners = new Dictionary<int, int>();
	float timeWrongMessageDisplayed = 3f;
	#endregion

	private void Awake()
	{
		#region Adding the right awnsers to the dictonary.
		rightAwners.Add(0, 7);
		rightAwners.Add(1, 3);
		rightAwners.Add(2, 8);
		rightAwners.Add(3, 5);
		rightAwners.Add(4, 1);
		#endregion
	}

	public void CheckAwnser()
	{
		if (needsAwnser)
		{			

			int awnser = conveyorBeltMovement.currentPlatformIndex;

			if (awnser == rightAwners[questionIndex])
			{
				needsAwnser = false;
				dialogueManager.Read();
			}
			else
			{
				StartCoroutine(DisplayWrongAwnserMessage(dialogueManager.CurrentDialogue));
			}
		}
	}

	private IEnumerator DisplayWrongAwnserMessage(string lastQuestion)
	{
		dialogueManager.dialogueText.color = Color.red;
		dialogueManager.dialogueText.text = wrongAwnserMessage;

		yield return new WaitForSeconds(timeWrongMessageDisplayed);

		dialogueManager.dialogueText.color = Color.cyan;
		dialogueManager.dialogueText.text = lastQuestion;

	}
}