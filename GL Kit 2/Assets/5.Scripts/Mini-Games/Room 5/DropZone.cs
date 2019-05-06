using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
	public GearType NeededType{ get; set; }

	public bool IsOccupied { get; private set; }
	public static bool IsCombinationRight { get; private set; } = true;	

	private Transform gear = null;
	private static int occupiedPlaces = 0;
	public static int OccupiedPlaces => occupiedPlaces;
	
	public void Occupy(Transform gear)
	{
		if (IsOccupied)
		{
			return;
		}

		this.gear = gear;
		IsOccupied = true;

		RightSlotCheck(gear);
		occupiedPlaces++;
	}

	public void Unoccupy()
	{
		if (!IsOccupied)
		{
			return;
		}

		gear = null;
		IsOccupied = false;

		occupiedPlaces--;
	}

	private void RightSlotCheck(Transform gearToCheck)
	{
		if (gearToCheck.GetComponent<GearInformation>().GetGearType != NeededType)
		{
			IsCombinationRight = false;
		}
	}
}