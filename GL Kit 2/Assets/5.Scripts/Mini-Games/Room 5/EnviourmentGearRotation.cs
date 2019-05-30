using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class EnviourmentGearRotation : BetterMonoBehaviour
{
	[SerializeField] float gearRotationSpeed = 3f;
	[SerializeField] bool rotatingLeft = false;

	private void Update()
	{
		if(GearInformation.isAbleToRotate)
		{
			if (rotatingLeft)
			{
				CachedTransform.Rotate(Vector3.forward * (Time.deltaTime * gearRotationSpeed));
			}
			else
			{
				CachedTransform.Rotate(Vector3.back * (Time.deltaTime * gearRotationSpeed));
			}
		}
	}
}
