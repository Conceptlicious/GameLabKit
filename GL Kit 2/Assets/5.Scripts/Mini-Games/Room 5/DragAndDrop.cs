using UnityEngine.EventSystems;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias
//Overview: This is used for dragging and dropping the gears in room 5, it calls the DropZone script so
//when you start or stop dragging the right dropzones are (un)occupied.
//Usage: On every IneractibleGear.
//--------------------------------------------------

public class DragAndDrop : BetterMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Vector3 BeginPosition { get; private set; } = Vector3.zero;
	[HideInInspector] public bool isObjectInGrid = false;
	[HideInInspector] public bool isAbleToMove = true;
	private static Canvas canvasRoom5;
	private static Plane planeRoom5;

	private void Start()
	{
		BeginPosition = CachedTransform.position;
		isAbleToMove = true;

		canvasRoom5 = GameObject.FindGameObjectWithTag("DragAndDropCanvas").GetComponent<Canvas>();

		planeRoom5 = new Plane();
		planeRoom5.Set3Points
			(canvasRoom5.transform.TransformPoint(new Vector3(0, 0)),
			canvasRoom5.transform.TransformPoint(new Vector3(0, 1)),
			canvasRoom5.transform.TransformPoint(new Vector3(1, 0)));
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (isObjectInGrid)
		{
			DropZone.combinationIsRight = true;
			DropZone dropZone = GridHandler.Instance.GetDropZoneUnder(CachedTransform as RectTransform);

			if (dropZone != null)
			{
				dropZone.Unoccupy();
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		//CachedTransform.position = eventData.position;
		if (isAbleToMove)
		{
			Ray ray = canvasRoom5.worldCamera.ScreenPointToRay(eventData.position);

			if(planeRoom5.Raycast(ray, out float hitDistance))
			{
				CachedTransform.position = ray.GetPoint(hitDistance);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DropZone dropZone = GridHandler.Instance.GetDropZoneUnder(CachedTransform as RectTransform);

		if (dropZone != null && !dropZone.IsOccupied)
		{
			float x = dropZone.CachedTransform.position.x;
			float y = dropZone.CachedTransform.position.y;
			float z = BeginPosition.z;
			CachedTransform.position = new Vector3(x, y, z);

			isObjectInGrid = true;
			dropZone.Occupy(CachedTransform);
		}
		else
		{
			CachedTransform.position = BeginPosition;
		}
	}
}