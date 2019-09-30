using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(MonoBehaviour))]
public class MediumButton : BetterMonoBehaviour
{
    public static Sprite selectedMediumSprite;
    private const string PUZZLE_IN_PROGRESS_WARNING = "You can't switch level when there is a puzzle in progress"; 

    public Sprite mediumSprite = null;
    [SerializeField] private MediumTemplate mediumInformation;
    [SerializeField] private GameObject mediumPuzzle;
    private Button mediumButton = null;

    private void Start()
    {
        mediumButton = GetComponent<Button>();
        mediumSprite = mediumInformation.icon;

        mediumButton.onClick.AddListener(() => SelectSprite(mediumSprite));
        mediumButton.onClick.AddListener(() => ShowNewPuzzle(mediumPuzzle));
    }

    private void SelectSprite(Sprite selectedSprite)
    {
        if (!MediumButtonManager.Instance.MinigameIsWon)
        {
            return;
        }

        selectedMediumSprite = selectedSprite;
        EventManager.Instance.RaiseEvent(new NextRoomEvent());
        EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Medium));
    }

    public void ShowNewPuzzle(GameObject currentPuzzle)
    {
        if(PuzzleManager.Instance.IsPuzzleInProgress)
        {
            PuzzleManager.Instance.DisplayPieceText(Color.yellow, PUZZLE_IN_PROGRESS_WARNING);
            return;
        }

        foreach (GameObject puzzle in MediumButtonManager.Instance.Puzzles)
        {
            puzzle.SetActive(false);
        }

        currentPuzzle.SetActive(true);

        PuzzleManager.Instance.SetupNewPuzzle(currentPuzzle);
    }
}
