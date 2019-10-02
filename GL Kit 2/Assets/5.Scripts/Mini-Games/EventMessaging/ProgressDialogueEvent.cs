using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class ProgressDialogueEvent : GameLabEvent
{
	private static bool shouldLoop;
	private static int roomPartID;
	public int RoomPartID => roomPartID;

	//<summary>
	// If not told different if always goes to the next knot and doesn't break out of a loop.	// 
	//</summary>
	public ProgressDialogueEvent(bool nextKnot = true, bool breakLoop = false)
	{
		if(breakLoop)
		{
			shouldLoop = false;
		}

		if(shouldLoop)
		{
			return;
		}

		if (nextKnot)
		{
			roomPartID++;
			return;
		}

		shouldLoop = true;
	}
}
