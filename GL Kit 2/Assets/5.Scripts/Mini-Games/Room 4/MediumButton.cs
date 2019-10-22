using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(Button))]
public class MediumButton : BetterMonoBehaviour
{
	private const string KNOT_NAME = "Part5";

	[SerializeField] private MediumTemplate mediumInformation;
	[SerializeField] private GameObject mediumPuzzle;
	private Button mediumButton = null;

	private void Start()
	{
		mediumButton = GetComponent<Button>();

		mediumButton.onClick.AddListener(() => SelectSprite());
		mediumButton.onClick.AddListener(() => ShowNewPuzzle(mediumPuzzle));
	}

	private void SelectSprite()
	{
		if (!MediumUIHandler.Instance.MinigameIsWon)
		{
			return;
		}

		DialogueManager.Instance.CurrentDialogue.Reset(KNOT_NAME);
		MediumUIHandler.Instance.SelectMedium(mediumInformation.name, mediumInformation.icon);
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	public void ShowNewPuzzle(GameObject currentPuzzle)
	{
		if(MediumUIHandler.Instance.MinigameIsWon)
		{
			return;
		}

		foreach (GameObject puzzle in PuzzleManager.Instance.puzzles)
		{
			puzzle.SetActive(false);
		}
		currentPuzzle.SetActive(true);

		PuzzleManager.Instance.SetupNewPuzzle(currentPuzzle);
		MediumUIHandler.Instance.OpenScreen(mediumInformation.name, mediumButton);
	}
}
