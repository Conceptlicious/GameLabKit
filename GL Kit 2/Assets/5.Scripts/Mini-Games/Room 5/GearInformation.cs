using System.Collections;
using UnityEngine;
using GameLab;

public enum GearType
{
	NONE,

	Exploration,
	Fantasy,
	Creativity,

	Communication,
	Cooperation,
	Competition,

	Goals,
	Obstacles,
	Stategy,

	Learing,
	Rhythm,
	Collection
};

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is used for knowing what the gear should do visually and what type of gear it is.
//The type, speed and direction are set in the inspector.
//Usage: This script is on every Intractable gear.
//--------------------------------------------------

public class GearInformation : BetterMonoBehaviour
{
	[HideInInspector] public static bool isAbleToRotate = false;

	[SerializeField] private GearType gearType = default;
	public GearType GetGearType => gearType;

	[SerializeField] private float timeForRotationToStop = 2f;

	public void StopGearRotationMethod(GameObject objectToCheck)
	{
		if (objectToCheck.activeInHierarchy)
		{
			StartCoroutine(StopGearRotation());
		}
	}

	private IEnumerator StopGearRotation()
	{
		ButtonManager.Instance.DisableButtons();

		yield return new WaitForSeconds(timeForRotationToStop);

		isAbleToRotate = false;

		transform.position = GetComponent<DragAndDrop>().BeginPosition;

		DropZone.combinationIsRight = true;

		foreach (DropZone dropZone in UIHandler.Instance.CurrentFunTypeTab.GetComponentsInChildren<DropZone>())
		{
			dropZone.Unoccupy();
		}

		ButtonManager.Instance.EnableButtons();
	}
}