using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(MonoBehaviour))]
public class MediumButton : BetterMonoBehaviour
{
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

    private void SelectSprite(Sprite newSprite)
    {
        if (!MediumButtonManager.Instance.MinigameIsWon)
        {
            return;
        }

        MediumButtonManager.Instance.selectedMediumSprite = newSprite;
    }

    public void ShowNewPuzzle(GameObject currentPuzzle)
    {
        foreach (GameObject puzzle in MediumButtonManager.Instance.Puzzles)
        {
            puzzle.SetActive(false);
        }

        currentPuzzle.SetActive(true);

        PuzzleManager.Instance.SetupNewPuzzle(currentPuzzle);
    }
}
