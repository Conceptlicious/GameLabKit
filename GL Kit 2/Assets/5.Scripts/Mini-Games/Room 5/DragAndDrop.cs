using UnityEngine;
using GameLab;
using UnityEngine.EventSystems;

public class DragAndDrop : BetterMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Vector3 BeginPosition { get; private set; } = Vector3.zero;
	[HideInInspector] public bool isObjectInGrid = false;
	private static GameObject canvas;

	private void Start()
	{
		BeginPosition = CachedTransform.position;
		canvas = FindObjectOfType<Canvas>().gameObject;
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
		Ray ray = Camera.main.ScreenPointToRay(eventData.position);
		transform.position = new Vector3(ray.origin.x, ray.origin.y, BeginPosition.z);
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