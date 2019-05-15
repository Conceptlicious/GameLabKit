using UnityEngine.EventSystems;
using UnityEngine;
using GameLab;

public class SwipeInput : Singleton<SwipeInput>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 beginSwipePosition = Vector3.zero;
	private Vector3 endSwipePosition = Vector3.zero;
	private float deadZone = 0f;

	private void Start()
	{
		deadZone = Screen.width * 0.1f;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		beginSwipePosition = transform.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		float x = eventData.position.x;
		float y = beginSwipePosition.y;
		float z = beginSwipePosition.z;

		transform.position = new Vector3 (x, y, z);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		endSwipePosition = transform.position;

		if (endSwipePosition.x > beginSwipePosition.x + deadZone)
		{
			ConveyorBeltMovement.Instance.Next();
		}
		else if (endSwipePosition.x < beginSwipePosition.x - deadZone)
		{
			ConveyorBeltMovement.Instance.Previous();
		}

		transform.position = beginSwipePosition;
	}
}