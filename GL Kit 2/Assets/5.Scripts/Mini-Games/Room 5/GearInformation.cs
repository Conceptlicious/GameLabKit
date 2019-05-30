using System.Collections;
using UnityEngine;
using GameLab;

public enum GearType
{
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

public class GearInformation : BetterMonoBehaviour
{
	[HideInInspector] public static bool isAbleToRotate = false;

	[SerializeField] private GearType gearType = default;
	public GearType GetGearType => gearType;

	[SerializeField] private float timeForRotationToStop = 2f;
	[SerializeField] private int rotationSpeed = 50;
	[SerializeField] private bool rotatingLeft = false;

	public void StopGearRotationMethod(GameObject objectToCheck)
	{
		if (objectToCheck.activeInHierarchy)
		{
			StartCoroutine(StopGearRotation());
		}
	}

	private IEnumerator StopGearRotation()
	{
		yield return new WaitForSeconds(timeForRotationToStop);
		isAbleToRotate = false;

		transform.position = GetComponent<DragAndDrop>().BeginPosition;
		DropZone.isCombinationRight = true;

		foreach (DropZone dropZone in UIHandler.Instance.DropZones)
		{
			dropZone.Unoccupy();
		}

	}

	private void Update()
	{
		if (isAbleToRotate)
		{
			if (!rotatingLeft)
			{
				CachedTransform.Rotate(Vector3.forward * (Time.deltaTime * rotationSpeed));
			}
			else
			{
				CachedTransform.Rotate(Vector3.back * (Time.deltaTime * rotationSpeed));
			}
		}
	}
}