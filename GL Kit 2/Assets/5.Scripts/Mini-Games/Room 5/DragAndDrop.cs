using UnityEngine;
using GameLab;
using UnityEngine.EventSystems;

public class DragAndDrop : BetterMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Vector3 BeginPosition { get; private set; } = Vector3.zero;
	[HideInInspector] public bool isObjectInGrid = false;
	[HideInInspector] public bool isAbleToMove = true; 
	private static Canvas canvasRoom5;

	private void Start()
	{
		BeginPosition = CachedTransform.position;
		isAbleToMove = true;
		canvasRoom5 = GameObject.FindGameObjectWithTag("DragAndDropCanvas").GetComponent<Canvas>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (isObjectInGrid)
		{
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
			Vector2 gearPosition;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoom5.transform as RectTransform,
				eventData.position, canvasRoom5.worldCamera, out gearPosition);
			transform.position = canvasRoom5.transform.TransformPoint(gearPosition);
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