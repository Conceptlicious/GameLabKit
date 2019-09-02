﻿using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Alex and some additions from Mathias
//Overview: This script is used to know if a dropzone is occupied or not. When a gear is dropped it gets occupied
//and when it's dragged out it gets unoccupied.
//Usage: it is on every interactable slot.
//--------------------------------------------------

public class DropZone : BetterMonoBehaviour
{
	[SerializeField] private GearType neededType;

	public static int OccupiedPlaces { get; private set; } = 0;
	[HideInInspector] public static bool combinationIsRight = true;

	public bool IsOccupied { get; private set; }
	private Transform gear = null;


	public void Occupy(Transform gear)
	{
		if (IsOccupied)
		{
			return;
		}

		this.gear = gear;
		IsOccupied = true;

		RightSlotCheck(gear);
		OccupiedPlaces++;
	}

	public void Unoccupy()
	{
		if (!IsOccupied)
		{
			if (OccupiedPlaces != 0)
			{
				OccupiedPlaces = 0;
			}
			return;
		}

		gear = null;
		IsOccupied = false;

		OccupiedPlaces--;
	}

	private void RightSlotCheck(Transform gearToCheck)
	{
		if (gearToCheck.GetComponent<GearInformation>().GetGearType != neededType)
		{
			combinationIsRight = false;
		}
	}
}