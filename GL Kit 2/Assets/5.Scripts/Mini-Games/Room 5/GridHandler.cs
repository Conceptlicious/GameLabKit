using GameLab;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridHandler : Singleton<GridHandler>
{
	public List<DropZone> DropZones { get; } = new List<DropZone>();

	private void Start()
	{
		SetVariables();
	}

	public DropZone GetDropZoneUnder(RectTransform rectTransform)
	{
		foreach (DropZone dropZone in DropZones)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(dropZone.transform as RectTransform, rectTransform.position))
			{
				return dropZone;
			}
		}

		return null;
	}

	public void EmptyDropZones()
	{
		foreach (DropZone dropZone in DropZones)
		{
			dropZone.Unoccupy();
		}
	}

	private void SetVariables()
	{		
		foreach (Transform child in transform)
		{
			DropZones.Add(child.GetComponent<DropZone>());
		}
	}
}