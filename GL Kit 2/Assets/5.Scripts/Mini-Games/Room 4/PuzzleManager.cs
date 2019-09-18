using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class PuzzleManager : Manager<PuzzleManager>
{
	public List<GameObject> Puzzles { get; private set; } = new List<GameObject>();
	public List<PuzzlePieceDrag> PuzzlePieces { get; private set; } = new List<PuzzlePieceDrag>();
	public List<PuzzlePieceSocket> PuzzlePieceSockets { get; private set; } = new List<PuzzlePieceSocket>();
	public int NumberOfPuzzlePiecesNeeded { get; private set; } = 0;
	private GameObject activePuzzle = null;

	private void Start()
	{
		foreach (Transform child in transform)
		{
			Puzzles.Add(child.gameObject);
		}
	}

	public void SetupNewPuzzle(GameObject puzzleToSetup)
	{
		PuzzlePieces.Clear();
		PuzzlePieceSockets.Clear();

		foreach(PuzzlePieceDrag puzzlePieceDrag in puzzleToSetup.GetComponentsInChildren<PuzzlePieceDrag>())
		{
			PuzzlePieces.Add(puzzlePieceDrag);
		}

		foreach(PuzzlePieceSocket puzzlePieceSocket in puzzleToSetup.GetComponentsInChildren<PuzzlePieceSocket>())
		{
			PuzzlePieceSockets.Add(puzzlePieceSocket);
			++NumberOfPuzzlePiecesNeeded;
		}
	}

	public List<PuzzlePieceSocket> GetPuzzlePieceSocketsUnder(RectTransform rectTransform)
	{
		List<PuzzlePieceSocket> puzzlePieceSocketsUnder = new List<PuzzlePieceSocket>();

		foreach(PuzzlePieceSocket puzzlePieceSocket in PuzzlePieceSockets)
		{
			if(RectTransformUtility.RectangleContainsScreenPoint(puzzlePieceSocket.transform as RectTransform, rectTransform.position))
			{
				puzzlePieceSocketsUnder.Add(puzzlePieceSocket);
			}
		}

		return puzzlePieceSocketsUnder;
	}
}