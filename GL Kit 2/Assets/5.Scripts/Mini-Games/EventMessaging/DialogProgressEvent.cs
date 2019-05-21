using System;
using GameLab;

public class DialogueProgressEvent : GameLabEvent
{
	// An int used to represent the index in an array of strings
	public int progressIndex = 0;
	public DialogueProgressEvent(int progressIndex)
	{
		this.progressIndex = progressIndex;
	} 
}

