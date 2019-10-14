using GameLab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestToPlayKnotEvent : GameLabEvent
{
	public RoomType RoomID { get; private set; }
	public string KnotToPlay { get; private set; }

	public RequestToPlayKnotEvent(RoomType roomID, string knotToPlay)
	{
		RoomID = roomID;
		KnotToPlay = knotToPlay;
	}
}
