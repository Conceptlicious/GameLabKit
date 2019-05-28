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
	[SerializeField] private GearType gearType = default;
	public GearType GetGearType => gearType;

	[SerializeField] private float timeForRotationToStop = 2f;
	[SerializeField] private int rotationSpeed = 50;
	[SerializeField] private bool invertRotation = false;
	[HideInInspector] public bool isAbleToRotate = false;

	public void StopGearRotationMethod(bool resetPosition)
	{
		StartCoroutine(StopGearRotation(resetPosition));
	}

	private IEnumerator StopGearRotation(bool resetPosition)
	{
		yield return new WaitForSeconds(timeForRotationToStop);
		isAbleToRotate = false;

		if (resetPosition)
		{
			transform.position = GetComponent<DragAndDrop>().BeginPosition;
			DropZone.isCombinationRight = true;

			foreach (DropZone dropZone in GridHandler.Instance.DropZones)
			{
				dropZone.Unoccupy();
			}
		}
		
	}

	private void Update()
	{
		if (isAbleToRotate)
		{
			if (!invertRotation)
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