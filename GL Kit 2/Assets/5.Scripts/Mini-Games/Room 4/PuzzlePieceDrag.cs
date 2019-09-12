using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GameLab;

public class PuzzlePieceDrag : BetterMonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	public PuzzlePieceType puzzlePieceType;

	private static Canvas Room4Canvas;
	private static Plane Room4Plane;

	[SerializeField] private float rightRotationZ;
	public Vector3 BeginPosition { get; private set; }
	[HideInInspector] public bool isInSocket = false;
	private bool isSelected = false;
	private float beginRotationZ = 0f;
	private int positionZ = 0;

	private void Start()
	{
		beginRotationZ = CachedTransform.eulerAngles.z;
		BeginPosition = CachedTransform.position;

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
				float x = ray.GetPoint(hitDistance).x;
				float y = ray.GetPoint(hitDistance).y;
				float z = positionZ;

				CachedTransform.position = new Vector3(x, y, z);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		PuzzlePieceSocket puzzlePieceSocket = PuzzleManager.Instance.GetPuzzlePieceSocketUnder(CachedTransform as RectTransform);

		if (puzzlePieceSocket != null)
		{
			puzzlePieceSocket.Occupy(CachedTransform);
		}
		Deselect();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//Is called when letting go of mouse button.

		if (!isSelected)
		{
			Select();
		}
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
		Debug.Log($"Deselected {name}");
		CachedRectTransform.localScale = Vector3.one;

		if (!isInSocket)
		{
			CachedRectTransform.eulerAngles = new Vector3(0, 0, beginRotationZ);
			CachedTransform.position = BeginPosition;
		}

		isSelected = false;
	}
}
