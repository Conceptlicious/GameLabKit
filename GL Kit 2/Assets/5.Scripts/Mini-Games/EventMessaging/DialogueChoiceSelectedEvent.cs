using GameLab;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoiceSelectedEvent : GameLabEvent
{
	public Choice DialogueChoice { get; private set; }

	public DialogueChoiceSelectedEvent(Choice dialogueChoice)
	{
		DialogueChoice = dialogueChoice;
	}
}