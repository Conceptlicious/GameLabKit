using GameLab;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMakeDialogueChoiceEvent : GameLabEvent
{
	public Choice DialogueChoice { get; private set; }

	public RequestMakeDialogueChoiceEvent(Choice dialogueChoice)
	{
		DialogueChoice = dialogueChoice;
	}
}
