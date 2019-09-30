using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is used to rotate the EnviourmentGear. In the inspector you can set which direction
//you want them to spin and how fast.
//Usage: On every EnviourmentGear.
//--------------------------------------------------

public class GearRotation : BetterMonoBehaviour
{
	private const float IMAGE_radius = 921.5f;

	[SerializeField, Tooltip("S = 6, M = 9, L = 12")] private float teethCount;
	public float TeethCount => teethCount;
	[SerializeField] private bool determinesSpeed;
	public bool DeterminesSpeed => determinesSpeed;
	[SerializeField] private float rotationSpeed;
	public float RotationSpeed => rotationSpeed;

	[SerializeField] GearRotation nextGearInLine;
	[SerializeField] private bool rotatingLeft = false;

	private void Update()
	{
		if (GearInformation.isAbleToRotate)
		{
			if (rotatingLeft)
			{
				CachedTransform.Rotate(Vector3.forward * (Time.deltaTime * rotationSpeed));
			}
			else
			{
				CachedTransform.Rotate(Vector3.back * (Time.deltaTime * rotationSpeed));
			}
		}
	}

	public void CalculateRotaionSpeed(GearRotation drivingGear = null)
	{
		if (!determinesSpeed && drivingGear != null)
		{
			float calculatedRotationSpeed = drivingGear.RotationSpeed * drivingGear.TeethCount / teethCount;
			rotationSpeed = calculatedRotationSpeed;
		}

		nextGearInLine?.CalculateRotaionSpeed(this);
	}
}