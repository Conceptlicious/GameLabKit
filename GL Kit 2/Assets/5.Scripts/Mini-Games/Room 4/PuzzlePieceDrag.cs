using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GameLab;

public class PuzzlePieceDrag : BetterMonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	private static Canvas Room4Canvas;
	private static Plane Room4Plane;

	[SerializeField] private float rightRotationZ;
	public Vector3 BeginPosition { get; private set; }
	private bool isSelected = false;
	private bool isInSocket = false;
	private float beginRotationZ = 0f;

	private void Start()
	{
		beginRotationZ = transform.eulerAngles.z;

		Room4Canvas = GameObject.FindWithTag("Room 4 Canvas").GetComponent<Canvas>();

		Room4Plane = new Plane();
		Room4Plane.Set3Points
			(
			Room4Canvas.transform.TransformPoint(new Vector3(0, 0)),
			Room4Canvas.transform.TransformPoint(new Vector3(0, 1)),
			Room4Canvas.transform.TransformPoint(new Vector3(1, 0))
			);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (isSelected && !isInSocket)
		{
			Ray ray = Room4Canvas.worldCamera.ScreenPointToRay(eventData.position);

			if (Room4Plane.Raycast(ray, out float hitDistance))
			{
				CachedTransform.position = ray.GetPoint(hitDistance);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		// Dropzone check
		Deselect();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Select();
	}

	private void Select()
	{
		foreach (PuzzlePieceDrag puzzlePieceDrag in PuzzleManager.Instance.PuzzlePieces)
		{
			puzzlePieceDrag.Deselect();
		}

		CachedRectTransform.localScale = Vector3.one * 1.1f;
		CachedRectTransform.eulerAngles = new Vector3(0, 0, rightRotationZ);

		isSelected = true;
	}

	public void Deselect()
	{
		CachedRectTransform.localScale = Vector3.one;

		if (!isInSocket)
		{		
			CachedRectTransform.eulerAngles = new Vector3(0, 0, beginRotationZ);
		}

		isSelected = false;
	}
}
