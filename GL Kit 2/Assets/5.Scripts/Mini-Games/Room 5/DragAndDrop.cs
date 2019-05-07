using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Vector3 BeginPosition { get; private set; } = Vector3.zero;
	[HideInInspector] public bool isObjectInGrid = false;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!isObjectInGrid)
		{
			BeginPosition = transform.position;
		}
		else
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
		transform.position = Input.mousePosition;
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
				transform.position = dropZone.transform.position;
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