using GameLab;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridHandler : Singleton<GridHandler>
{
	public DropZone GetDropZoneUnder(RectTransform rectTransform)
	{
		foreach (DropZone dropZone in UIHandler.Instance.DropZones)
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
		foreach (DropZone dropZone in UIHandler.Instance.DropZones)
		{
			dropZone.Unoccupy();
		}
	}
}