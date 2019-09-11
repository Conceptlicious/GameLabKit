using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(MonoBehaviour))]
public class MediumButton : BetterMonoBehaviour
{
	[SerializeField] private MediumTemplate mediumInformation;
	[SerializeField] private GameObject mediumPuzzle;

	private Sprite mediumSprite = null;
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
		//Select a sprite that can be stand to the trophyHandler
	}

	public void ShowNewPuzzle(GameObject currentPuzzle)
	{
		foreach(GameObject puzzle in PuzzleManager.Instance.Puzzles)
		{
			puzzle.SetActive(false);
		}
		currentPuzzle.SetActive(true);


		PuzzleManager.Instance.SetupNewPuzzle(currentPuzzle);
	}
}
