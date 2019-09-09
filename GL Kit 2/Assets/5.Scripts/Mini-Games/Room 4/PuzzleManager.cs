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
	public List<GameObject> PuzzlePieceSockets { get; private set; } = new List<GameObject>();
	private GameObject activePuzzle = null;

	private void Start()
	{
		foreach (Transform child in transform)
		{
			Puzzles.Add(child.gameObject);
		}
	}

	public void SetupNewPuzzle()
	{
		PuzzlePieces.Clear();

		foreach(PuzzlePieceDrag puzzlePieceDrag in gameObject.GetComponentsInChildren<PuzzlePieceDrag>())
		{
			PuzzlePieces.Add(puzzlePieceDrag);
		}

		// Remove all the old puzzle pieces and puzzle piece sockets.

		// Add all the new puzzle pieces and puzzle piece sockets

		/*
		foreach(Transform child in transform)
		{
			PuzzlePieceDrag puzzlePieceDrag = child.GetComponentInChildren<PuzzlePieceDrag>();

			if(puzzlePieceDrag != null)
			{
				Debug.Log($"{puzzlePieceDrag.name} is added to puzzle pieces.");
				PuzzlePieces.Add(puzzlePieceDrag.gameObject);
				continue;
			}

			Debug.Log($"{puzzlePieceDrag.name} is not added to puzzle pieces.");
		}
		*/
	}
}