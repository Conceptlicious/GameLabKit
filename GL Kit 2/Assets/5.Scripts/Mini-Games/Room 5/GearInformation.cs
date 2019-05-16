﻿using System.Collections;
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

	[SerializeField] private bool invertRotation = false;
	[SerializeField] private int rotationSpeed = 50;
	[SerializeField] private float timeForRotationToStop = 2f;
	[HideInInspector] public bool isAbleToRotate = false;

	public void StopGearRotationMethod()
	{
		StartCoroutine(StopGearRotation());
	}

	private IEnumerator StopGearRotation()
	{
		yield return new WaitForSeconds(timeForRotationToStop);
		isAbleToRotate = false;
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