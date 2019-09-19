using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class PuzzleManager : Manager<PuzzleManager>
{
    private const string COMPLETED_PUZZLE_MESSAGE = "Congratulations you unlocked the next puzzle!";

    public List<PuzzlePieceDrag> PuzzlePieces { get; private set; } = new List<PuzzlePieceDrag>();
    public List<PuzzlePieceSocket> PuzzlePieceSockets { get; private set; } = new List<PuzzlePieceSocket>();
    public int PiecesInSocketNeeded { get; private set; } = 0;
    [SerializeField] private Text pieceText;
    private int piecesInSocket = 0;
    private GameObject activePuzzle = null;

    private void Start()
    {
        DisplayPieceText();

        foreach (Transform child in transform)
        {
           MediumButtonManager.Instance.Puzzles.Add(child.gameObject);
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

    public void DisplayPieceText(Color textColor, string textToDisplay = null)
    {
        pieceText.color = textColor;
        pieceText.text = textToDisplay;
    }

    public void DisplayPieceText(string textToDisplay = null)
    {
        pieceText.color = Color.black;
        pieceText.text = textToDisplay;
    }

    public void NewPuzzlePieceInSocket()
    {
        ++piecesInSocket;

        if (piecesInSocket == PiecesInSocketNeeded)
        {
            DisplayPieceText(Color.green, COMPLETED_PUZZLE_MESSAGE);
            MediumButtonManager.Instance.EnableNextButton();
        }
    }
}