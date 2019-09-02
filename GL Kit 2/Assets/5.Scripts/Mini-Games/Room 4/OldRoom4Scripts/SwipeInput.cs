using UnityEngine.EventSystems;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is used in room four, it handles the swipe input on the conveyor belt.
//Usage: It is used once on the swipeObject in room 4.
//--------------------------------------------------

public class SwipeInput : Singleton<SwipeInput>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private const float BEGIN_Y_POSITION = 394.1201f;
	private const float BEGIN_Z_POSITION = -.1f;

	[SerializeField] private Canvas canvasRoom4;
	private Vector3 snapPosition = Vector3.zero;
	private Vector3 beginSwipePosition = Vector3.zero;
	private Vector3 endSwipePosition = Vector3.zero;
	private float deadZone = 0f;

	private void Start()
	{
		snapPosition = transform.position;
		deadZone = Screen.width * 0.1f;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoom4.transform as RectTransform,
			eventData.position, canvasRoom4.worldCamera, out Vector2 swipePosition);

		float x = swipePosition.x;
		float y = BEGIN_Y_POSITION;
		float z = BEGIN_Z_POSITION;

		beginSwipePosition = new Vector3(x, y, z);
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoom4.transform as RectTransform,
			eventData.position, canvasRoom4.worldCamera, out Vector2 swipePosition);

		float x = swipePosition.x;
		float y = BEGIN_Y_POSITION;
		float z = BEGIN_Z_POSITION;

		transform.position = new Vector3(x, y, z);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		endSwipePosition = transform.position;

		if (endSwipePosition.x > (beginSwipePosition.x + deadZone))
		{
			ConveyorBeltMovement.Instance.Next();
		}
		else if (endSwipePosition.x < (beginSwipePosition.x - deadZone))
		{
			ConveyorBeltMovement.Instance.Previous();
		}

		transform.position = snapPosition;
		endSwipePosition = Vector3.zero;
	}
}