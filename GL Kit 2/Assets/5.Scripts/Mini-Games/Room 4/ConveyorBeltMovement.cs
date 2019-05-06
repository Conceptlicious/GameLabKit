using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltMovement : MonoBehaviour
{
	#region Varibles	
	[HideInInspector] public int currentPlatformIndex;

	[SerializeField] private List<GameObject> platforms = new List<GameObject>();

	private Vector3 beginSwipePosition, endSwipePosition;
	private float minimumHeight = 104f;
	#endregion

	private void Start()
	{
		PlatformInformation currentPlatformInformation = platforms[currentPlatformIndex].GetComponent<PlatformInformation>();
		currentPlatformInformation.LoadInformation(currentPlatformIndex);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			beginSwipePosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			endSwipePosition = Input.mousePosition;

			if (endSwipePosition.y > minimumHeight)
			{
				if (endSwipePosition.x > beginSwipePosition.x)
				{
					Next();
				}
				else if (endSwipePosition.x < beginSwipePosition.x)
				{
					Previous();
				}
			}
		}
	}

	private void Next()
	{
		currentPlatformIndex = (currentPlatformIndex + 1) % platforms.Count;		

		PlatformInformation currentPlatformInformation = platforms[currentPlatformIndex].GetComponent<PlatformInformation>();
		currentPlatformInformation.LoadInformation(currentPlatformIndex);
	}

	private void Previous()
	{
		if (currentPlatformIndex <= 0)
		{
			currentPlatformIndex = platforms.Count - 1;
		}
		else
		{
			currentPlatformIndex = (currentPlatformIndex - 1) % platforms.Count;
		}

		PlatformInformation currentPlatformInformation = platforms[currentPlatformIndex].GetComponent<PlatformInformation>();
		currentPlatformInformation.LoadInformation(currentPlatformIndex);
	}
}