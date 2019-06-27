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
