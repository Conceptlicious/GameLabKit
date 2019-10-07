using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class MediumUIHandler : Manager<MediumUIHandler>
{
	private const int MAX_AMOUNT_OF_ACTIVE_BUTTONS = 8;
	private const string PUZZLE_IN_PROGRESS_WARNING = "You can't close when there is a puzzle in progress";
	private const string WON_MINIGAME_MESSAGE = "<color=#00ff00>Congratulations you won the mini game.</color>" +
		"Click on the medium you want to send to the white room.";

	public bool MinigameIsWon { get; private set; } = false;
	public string ExtendedDescription { get; private set; }
	

	public Button closeScreenButton;
	[SerializeField] private GameObject pieceTextHolder;
	[SerializeField] private Image completeButtons;
	[HideInInspector] public int activeButtons = 0;	
	private List<Button> buttons = new List<Button>();
	private Text pieceText = null;

	private void Start()
	{
		foreach (Button button in GetComponentsInChildren<Button>())
		{
			buttons.Add(button);
			button.interactable = false;
		}

		pieceText = pieceTextHolder.GetComponentInChildren<Text>();

		EnableNextButton();
		CloseScreen();

		closeScreenButton.onClick.AddListener(() => CloseScreen());
	}

	public void EnableNextButton()
	{
		if (activeButtons >= MAX_AMOUNT_OF_ACTIVE_BUTTONS)
		{
			MinigameIsWon = true;
			DisplayPieceText(WON_MINIGAME_MESSAGE);
			return;
		}

		buttons[activeButtons].interactable = true;
		++activeButtons;
	}

	public void DisplayPieceText(Color textColor, string textToDisplay)
	{
		pieceText.color = textColor;
		pieceText.text = textToDisplay;
	}

	public void DisplayPieceText(string textToDisplay = null)
	{
		pieceText.color = Color.black;
		pieceText.text = textToDisplay;
	}

	public void OpenScreen(string mediumName, string mediumDescription)
	{
		closeScreenButton.gameObject.SetActive(true);
		pieceTextHolder.SetActive(true);

		ExtendedDescription = mediumDescription;

		pieceText.text = mediumName;
	}

	public void CloseScreen()
	{
		if(PuzzleManager.Instance.IsPuzzleInProgress)
		{
			DisplayPieceText(new Color32(252, 227, 0, 255), PUZZLE_IN_PROGRESS_WARNING);
			return;
		}

		foreach (GameObject puzzle in PuzzleManager.Instance.puzzles)
		{
			puzzle.SetActive(false);
		}

		closeScreenButton.gameObject.SetActive(false);
		pieceTextHolder.SetActive(false);
	}

	public void GameFinished()
	{
		completeButtons.gameObject.SetActive(true);
		DialogueProgression.Instance.RemoveChildren();
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
		DialogueProgression.Instance.RemoveChildren();
		ProgressDialogueEvent.ResetKnotID(4);
	}
}
