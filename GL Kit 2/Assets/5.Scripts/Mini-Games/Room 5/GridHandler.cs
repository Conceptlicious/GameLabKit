using GameLab;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridHandler : Singleton<GridHandler>
{
	[SerializeField] private List<DropZone> dropZones = new List<DropZone>();
	[SerializeField] private UIHandler uiHandler = null;

	private int resetGearsDelay = 3;

	public DropZone GetDropZoneUnder(RectTransform rectTransform)
	{
		foreach(DropZone dropZone in dropZones)
		{
			if(RectTransformUtility.RectangleContainsScreenPoint(dropZone.transform as RectTransform, rectTransform.position))
			{
				return dropZone;
			}
		}

		return null;
	}	
	
	public void EmptyDropZones()
	{
		foreach(DropZone dropZone in dropZones)
		{
			dropZone.Unoccupy();
		}

		StartCoroutine(ResetGearsPosition());
	}

	private IEnumerator ResetGearsPosition()
	{
		yield return new WaitForSeconds(resetGearsDelay);

		foreach (Transform child in uiHandler.GearsObject)
		{
			child.position = child.GetComponent<DragAndDrop>().BeginPosition;
			child.GetComponent<GearInformation>().isAbleToRotate = false;
		}
	}
}