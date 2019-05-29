using GameLab;
using UnityEngine;
using System.Collections.Generic;

public class ConveyorBeltMovement : Singleton<ConveyorBeltMovement>
{	
	private const float minimumHeight = 169f;

	public int CurrentPlatformIndex { get; private set; }
	private List<Transform> platforms = new List<Transform>();

	private void Start()
	{
		foreach(Transform child in transform)
		{
			platforms.Add(child);
		}

		Debug.Log(platforms.Count);

		DisplayPlatformInformation.Instance.LoadInformation(CurrentPlatformIndex);		
	}	

	public void Next()
	{
		CurrentPlatformIndex = (CurrentPlatformIndex + 1) % platforms.Count;
		DisplayPlatformInformation.Instance.LoadInformation(CurrentPlatformIndex);
	}

	public void Previous()
	{
		if (CurrentPlatformIndex <= 0)
		{
			CurrentPlatformIndex = platforms.Count - 1;
		}
		else
		{
			CurrentPlatformIndex = (CurrentPlatformIndex - 1) % platforms.Count;
		}

		DisplayPlatformInformation.Instance.LoadInformation(CurrentPlatformIndex);
	}	
}