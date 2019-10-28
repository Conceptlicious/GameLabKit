using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class MoveDrGameLabEvent : GameLabEvent
{
	public enum MovementDirection
	{
		MoveInToScreen,
		MoveOutOffScreen
	}

	public bool MoveIn { get; private set; }

	public MoveDrGameLabEvent(bool moveIn = true)
	{
		MoveIn = moveIn;
	}
}