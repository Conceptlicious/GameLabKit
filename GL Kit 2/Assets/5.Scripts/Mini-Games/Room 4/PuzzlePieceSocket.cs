using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class PuzzlePieceSocket : BetterMonoBehaviour
{
	[SerializeField] PuzzlePieceType puzzlePieceType;

	private bool isOccupied = false;

	public void Occupy(Transform puzzlePiece)
	{
		PuzzlePieceDrag puzzlePieceDrag = puzzlePiece.GetComponent<PuzzlePieceDrag>();

		if (isOccupied)
		{
			return;
		}

		if (IsRightPuzzlePiece(puzzlePieceDrag))
		{
			puzzlePiece.position = CachedTransform.position;

			puzzlePieceDrag.isInSocket = true;
		}
	}

	private bool IsRightPuzzlePiece(PuzzlePieceDrag puzzlePieceDrag)
	{
		if (puzzlePieceDrag.puzzlePieceType != puzzlePieceType)
		{
			return false;
		}

		return true;
	}
}