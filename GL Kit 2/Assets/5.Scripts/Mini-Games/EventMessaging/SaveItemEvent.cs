﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public enum RoomType
{
	TargetAudience,
	Goals,
	Genre,
	Medium,
	Dynamics,
	ArtStyle
};

public class SaveItemEvent : GameLabEvent
{
	public RoomType CurrentRoomType { get; private set; }
	public Component CurrentInvoker { get; private set; }

	public SaveItemEvent(RoomType newRoomType, Component newInvoker)
	{
		CurrentRoomType = newRoomType;
		CurrentInvoker = newInvoker;
	}
}