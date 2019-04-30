using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
	#region Varibles
	[HideInInspector] public bool needsAwnser = false;
	[HideInInspector] public int questionIndex = -1;

	[SerializeField] DialogueManager dialogueManager;
	[SerializeField] ConveyorBeltMovement conveyorBeltMovement;

	Dictionary<int, int> rightAwners = new Dictionary<int, int>();
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
				dialogueManager.dialogueText.color = Color.white;
				dialogueManager.dialogueText.text = "This is not the right awnser.\n Please try again.";
			}
		}
	}
}