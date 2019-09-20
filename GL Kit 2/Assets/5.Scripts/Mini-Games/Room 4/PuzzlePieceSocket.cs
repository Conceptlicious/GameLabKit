using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class PuzzlePieceSocket : BetterMonoBehaviour
{
	[SerializeField] private PuzzlePieceType neededPuzzlePieceType;
	public PuzzlePieceType NeededPuzzlePieceType => neededPuzzlePieceType;

	private bool isOccupied = false;

	public void Occupy(Transform puzzlePiece)
	{
		PuzzlePieceDrag puzzlePieceDrag = puzzlePiece.GetComponent<PuzzlePieceDrag>();

		if (isOccupied)
		{
			return;
		}

		isOccupied = true;

		puzzlePiece.position = CachedTransform.position;

		puzzlePieceDrag.isInSocket = true;
		puzzlePieceDrag.canBeSelected = false;

		PuzzleManager.Instance.NewPuzzlePieceInSocket();
	}
}