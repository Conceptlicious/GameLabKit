using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class TrophyDrag : BetterMonoBehaviour, IDragHandler, IEndDragHandler
{
	private static Canvas canvasWhiteRoom;
	private static Plane planeWhiteRoom;

	public Vector3 BeginPosition { get; private set; } = Vector3.zero;
	public RoomType roomType;
	private Vector2[] beginAnchorPoints = new Vector2[2];

	private void Start()
	{
		BeginPosition = CachedTransform.position;
		beginAnchorPoints[0] = CachedRectTransform.anchorMin;
		beginAnchorPoints[1] = CachedRectTransform.anchorMax;

		canvasWhiteRoom = GameObject.FindGameObjectWithTag("WhiteRoomCanvas").GetComponent<Canvas>();

		planeWhiteRoom = new Plane();
		planeWhiteRoom.Set3Points
			(
			canvasWhiteRoom.transform.TransformPoint(new Vector3(0, 0)),
			canvasWhiteRoom.transform.TransformPoint(new Vector3(0, 1)),
			canvasWhiteRoom.transform.TransformPoint(new Vector3(1, 0))
			);
	}	

	public void OnDrag(PointerEventData eventData)
	{
		Ray ray = canvasWhiteRoom.worldCamera.ScreenPointToRay(eventData.position);

		if(planeWhiteRoom.Raycast(ray, out float hitDistance))
		{
			CachedTransform.position = ray.GetPoint(hitDistance);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		TrophySocket trophySocket = TrophyGridManager.Instance.GetThrophySocketUnder(CachedTransform as RectTransform);

		if (trophySocket != null)
		{
			trophySocket.Occupy(CachedTransform);
			return;
		}
	}
}