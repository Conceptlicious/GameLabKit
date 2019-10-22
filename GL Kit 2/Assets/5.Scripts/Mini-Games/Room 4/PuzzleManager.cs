using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class PuzzleManager : Manager<PuzzleManager>
{
	[HideInInspector] public List<GameObject> puzzles = new List<GameObject>();
	public List<PuzzlePieceDrag> PuzzlePieces { get; private set; } = new List<PuzzlePieceDrag>();
	public List<PuzzlePieceSocket> PuzzlePieceSockets { get; private set; } = new List<PuzzlePieceSocket>();
	public bool IsPuzzleInProgress { get; private set; } = false;
	[HideInInspector] public GameObject activePuzzle = null;
	private int PiecesInSocketNeeded = 0;
	private int piecesInSocket = 0;

	protected override void Awake()
	{
		base.Awake();

		foreach (Transform child in transform)
		{
			puzzles.Add(child.gameObject);
		}
	}

	public void SetupNewPuzzle(GameObject puzzleToSetup)
	{
		PuzzlePieces.Clear();
		PuzzlePieceSockets.Clear();

		foreach (PuzzlePieceDrag puzzlePieceDrag in puzzleToSetup.GetComponentsInChildren<PuzzlePieceDrag>())
		{
			PuzzlePieces.Add(puzzlePieceDrag);
		}

		foreach (PuzzlePieceSocket puzzlePieceSocket in puzzleToSetup.GetComponentsInChildren<PuzzlePieceSocket>())
		{
			PuzzlePieceSockets.Add(puzzlePieceSocket);
		}

		PiecesInSocketNeeded = PuzzlePieceSockets.Count;
		piecesInSocket = 0;
	}

	public List<PuzzlePieceSocket> GetPuzzlePieceSocketsUnder(RectTransform rectTransform)
	{
		List<PuzzlePieceSocket> puzzlePieceSocketsUnder = new List<PuzzlePieceSocket>();

		foreach (PuzzlePieceSocket puzzlePieceSocket in PuzzlePieceSockets)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(puzzlePieceSocket.transform as RectTransform, rectTransform.position))
			{
				puzzlePieceSocketsUnder.Add(puzzlePieceSocket);
			}
		}

		return puzzlePieceSocketsUnder;
	}

	public void NewPuzzlePieceInSocket()
	{
		IsPuzzleInProgress = true;
		++piecesInSocket;

		if (piecesInSocket == PiecesInSocketNeeded)
		{
			PuzzleCompleted();
		}		
	}

	private void PuzzleCompleted()
	{
		IsPuzzleInProgress = false;
		MediumUIHandler.Instance.EnableNextButton();

		DialogueManager.Instance.CurrentDialogue.CurrentKnot = activePuzzle.name;
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}
}