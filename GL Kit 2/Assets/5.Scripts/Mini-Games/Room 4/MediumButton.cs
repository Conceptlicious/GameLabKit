using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(Button))]
public class MediumButton : BetterMonoBehaviour
{
	public static Sprite selectedMediumSprite;

	[SerializeField] private MediumTemplate mediumInformation;
	[SerializeField] private GameObject mediumPuzzle;
	[SerializeField] private Image completeButtons;
	[HideInInspector] public Sprite mediumSprite = null;
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
		if (MediumUIHandler.Instance.MinigameIsWon)
		{
			selectedMediumSprite = selectedSprite;
			completeButtons.gameObject.SetActive(true);
			//EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
			//EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Medium));
		}
	}

	public void YesButton()
	{
		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Medium));
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		completeButtons.gameObject.SetActive(false);
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	public void NoButton()
	{
		completeButtons.gameObject.SetActive(false);
	}

	public void ShowNewPuzzle(GameObject currentPuzzle)
	{
		if(MediumUIHandler.Instance.activeButtons == 1)
		{
			//EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
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
