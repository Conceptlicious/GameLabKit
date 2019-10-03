using UnityEngine;
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
	[SerializeField] private GearType secondNeededType;

	public static int OccupiedPlaces { get; private set; } = 0;
	[HideInInspector] public static bool combinationIsRight = true;
	private const int MAX_OCCUPIED_PLACES = 3;

	[SerializeField] private int layer;
	public int Layer => layer;

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
		UpdateOccupiedPlaces(true);
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

		UpdateOccupiedPlaces(false);
	}

	private void RightSlotCheck(Transform gearToCheck)
	{
		if (gearToCheck.GetComponent<GearInformation>().GetGearType != neededType &&
			gearToCheck.GetComponent<GearInformation>().GetGearType != secondNeededType)
		{
			combinationIsRight = false;
		}
	}

	private void UpdateOccupiedPlaces(bool addOccupiedPlace)
	{
		if (addOccupiedPlace)
		{
			OccupiedPlaces++;
		}
		else
		{
			OccupiedPlaces--;
		}

		if (OccupiedPlaces == MAX_OCCUPIED_PLACES)
		{
			ButtonManager.Instance.StartRotationButton.gameObject.SetActive(true);
			return;
		}

		ButtonManager.Instance.StartRotationButton.gameObject.SetActive(false);
	}
}