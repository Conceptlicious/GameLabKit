using GameLab;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//--------------------------------------------------
//Produced by Alex
//Overview: This script is used for returning the drop zone which you are currently hovering over.
//Usage: It only gets called from other scripts.
//--------------------------------------------------

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