﻿using UnityEngine;

public class DropZone : MonoBehaviour
{

	public static int OccupiedPlaces { get; private set; } = 0;
	public static bool IsCombinationRight { get; private set; } = true;	

	[HideInInspector] public GearType neededType;
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
			IsCombinationRight = false;
		}
	}
}