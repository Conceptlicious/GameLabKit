using UnityEngine;
using GameLab;
using UnityEngine.EventSystems;

public class DragAndDrop : BetterMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Vector3 BeginPosition { get; private set; } = Vector3.zero;
	[HideInInspector] public bool isObjectInGrid = false;

	private void Start()
	{
		BeginPosition = transform.position;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{

		if (isObjectInGrid)
		{
			DropZone dropZone = GridHandler.Instance.GetDropZoneUnder(transform as RectTransform);

			if (dropZone != null)
			{
				dropZone.Unoccupy();
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{

		DropZone dropZone = GridHandler.Instance.GetDropZoneUnder(transform as RectTransform);

		if (dropZone != null)
		{
			if (dropZone.IsOccupied)
			{
				transform.position = BeginPosition;
			}
			else
			{
				float x = dropZone.transform.position.x;
				float y = dropZone.transform.position.y;
				float z = BeginPosition.z;
				transform.position = new Vector3(x, y, z);

				isObjectInGrid = true;
				dropZone.Occupy(transform);
			}
		}
		else
		{
			transform.position = BeginPosition;
		}
	}
}