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

    [TextArea][SerializeField] private string pieceInformation;
    public Vector3 BeginPosition { get; private set; }
    [HideInInspector] public bool isInSocket = false;
    [HideInInspector] public bool canBeSelected = true;
    private bool isSelected = false;
    private float beginRotationZ = 0f;

    private void Start()
    {
        beginRotationZ = Random.Range(0, 360);
        CachedRectTransform.eulerAngles = new Vector3(0, 0, beginRotationZ);

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
        Debug.Assert(PuzzleManager.Instance.PuzzlePieceSockets.Count > 0, "Socket list is empty");
        Debug.Assert(PuzzleManager.Instance.PuzzlePieces.Count > 0, "Piece list is empty");

        if (isSelected && !isInSocket)
        {
            Ray ray = Room4Canvas.worldCamera.ScreenPointToRay(eventData.position);

            if (Room4Plane.Raycast(ray, out float hitDistance))
            {
                float x = ray.GetPoint(hitDistance).x;
                float y = ray.GetPoint(hitDistance).y;
                float z = BeginPosition.z;

                CachedTransform.position = new Vector3(x, y, z);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (PuzzlePieceSocket puzzlePieceSocket in
            PuzzleManager.Instance.GetPuzzlePieceSocketsUnder(CachedRectTransform as RectTransform))
        {
            if (puzzlePieceSocket.NeededPuzzlePieceType == puzzlePieceType)
            {
                puzzlePieceSocket.Occupy(CachedTransform);
            }
        }

        Deselect();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelected && canBeSelected)
        {
            Select();
            PuzzleManager.Instance.DisplayPieceText(pieceInformation);
        }
    }

    private void Select()
    {
        foreach (PuzzlePieceDrag puzzlePieceDrag in PuzzleManager.Instance.PuzzlePieces)
        {
            puzzlePieceDrag.Deselect();
        }

        CachedRectTransform.localScale = Vector3.one * 1.1f;
        CachedRectTransform.eulerAngles = Vector3.zero;

        isSelected = true;
    }

    public void Deselect()
    {
        CachedRectTransform.localScale = Vector3.one;

        if (!isInSocket)
        {
            CachedRectTransform.eulerAngles = new Vector3(0, 0, beginRotationZ);
            CachedTransform.position = BeginPosition;
        }

        isSelected = false;
    }
}