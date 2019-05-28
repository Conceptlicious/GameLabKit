using UnityEngine;
using GameLab;

public class DropZone : BetterMonoBehaviour
{
	[HideInInspector] public GearType neededType;

	public static int OccupiedPlaces { get; private set; } = 0;
	[HideInInspector] public static bool isCombinationRight = true;	

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
			isCombinationRight = false;
		}
	}
}