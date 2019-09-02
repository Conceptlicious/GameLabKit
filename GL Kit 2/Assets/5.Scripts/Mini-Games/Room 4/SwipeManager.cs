using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class SwipeManager : Manager<SwipeManager>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 beginPosition = Vector3.zero;
	private Vector3 endPosition = Vector3.zero;
	private float deadZone = 0f;

	private void Start()
	{
		deadZone = Screen.width * 0.1f;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		beginPosition = CachedTransform.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		float x = eventData.position.x;
		float y = beginPosition.y;
		float z = beginPosition.z;

		CachedTransform.position = new Vector3(x, y, z);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		float x = eventData.position.x;
		float y = beginPosition.y;
		float z = beginPosition.z;

		endPosition = new Vector3(x, y, z);

		if (endPosition.x > beginPosition.x + deadZone)
		{
			ConvyerBeltManager.Instance.Next();
		}

		if(endPosition.x < beginPosition.x - deadZone)
		{
			ConvyerBeltManager.Instance.Previous();
		}

		transform.position = beginPosition;
	}
}
