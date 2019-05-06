using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class GearInformation : MonoBehaviour
{
	#region Variables
	[HideInInspector] public bool isAbleToRotate { get; set; } = false;

	[SerializeField] private bool invertRotation = false;
	[SerializeField] private GearType gearType = default;
	public GearType GetGearType => gearType;

	private int rotationSpeed = 50;
	private float timeForRotationToStop = 2f;
	#endregion

	private void Update()
	{
		if (isAbleToRotate)
		{
			if (!invertRotation)
			{
				transform.Rotate(Vector3.forward * (Time.deltaTime * rotationSpeed));
			}
			else
			{
				transform.Rotate(Vector3.back * (Time.deltaTime * rotationSpeed));
			}
		}
	}

	public void StopGearRotationMethod()
	{
		StartCoroutine(StopGearRotation());
	}

	private IEnumerator StopGearRotation()
	{		
		yield return new WaitForSeconds(timeForRotationToStop);
		isAbleToRotate = false;
	}
}