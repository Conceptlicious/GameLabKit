using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;
using System;

[DisallowMultipleComponent]
public class MediumUIHandler : Manager<MediumUIHandler>
{
	private const string KNOT_3_NAME = "Part3";
	private const string KNOT_4_NAME = "Part4";
	private const int MAX_AMOUNT_OF_ACTIVE_BUTTONS = 8;
	private const string PUZZLE_IN_PROGRESS_WARNING = "You can't close when there is a puzzle in progress";
	private const string KNOT_NAME_WEARABLE = "Wearable";
	private readonly Color32 FCE300 = new Color(252, 227, 0);

	public bool MinigameIsWon { get; private set; } = false;
	public Button closeScreenButton;
	[SerializeField] private GameObject pieceTextHolder;
	[HideInInspector] public int activeButtons = 0;	
	private List<Button> buttons = new List<Button>();
	private Image[] buttonIcons;
	private Text pieceText = null;

	private void Start()
	{
		SetVariables();
		EnableNextButton();
		CloseScreen();		
	}

	public void EnableNextButton()
	{
		if (activeButtons >= MAX_AMOUNT_OF_ACTIVE_BUTTONS)
		{
			MinigameIsWon = true; DialogueManager.Instance.CurrentDialogue.CurrentKnot = KNOT_4_NAME;
			MenuManager.Instance.OpenMenu<DialogueMenu>();
			return;
		}

		buttons[activeButtons].interactable = true;
		buttonIcons[activeButtons].color = Color.white;
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

	public void OpenScreen(string mediumName, Button pressedButton)
	{
		closeScreenButton.gameObject.SetActive(true);
		pieceTextHolder.SetActive(true);

		PuzzleManager.Instance.activePuzzle = pressedButton.gameObject;
		pieceText.text = mediumName;
	}

	public void CloseScreen()
	{
		if(PuzzleManager.Instance.IsPuzzleInProgress)
		{
			DisplayPieceText(FCE300, PUZZLE_IN_PROGRESS_WARNING);
			return;
		}

		foreach (GameObject puzzle in PuzzleManager.Instance.puzzles)
		{
			puzzle.SetActive(false);
		}

		closeScreenButton.gameObject.SetActive(false);
		pieceTextHolder.SetActive(false);
	}

	private void OnRoomTransitionFinished(FinishedRoomTransition eventData)
	{
		int currentRoomID = RoomManager.Instance.GetCurrentRoomID().z;

		if(currentRoomID == 4)
		{
			DialogueManager.Instance.SetCurrentDialogue(RoomType.Medium);
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
	}

	private void OnDialogueKnotCompleted(DialogueKnotCompletedEvent eventData)
	{
		if(eventData.Knot != KNOT_NAME_WEARABLE || eventData.CompletedRoomID != RoomType.Medium)
		{
			return;
		}

		DialogueManager.Instance.CurrentDialogue.CurrentKnot = KNOT_3_NAME;
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	private void SetVariables()
	{
		foreach (Button button in GetComponentsInChildren<Button>())
		{
			buttons.Add(button);
			button.interactable = false;
		}

		buttonIcons = new Image[buttons.Count];
		for (int i = 0; i < buttons.Count; i++)
		{
			buttonIcons[i] = buttons[i].transform.GetChild(0).GetComponent<Image>();
			buttonIcons[i].color = Color.black;
		}

		pieceText = pieceTextHolder.GetComponentInChildren<Text>();

		closeScreenButton.onClick.AddListener(() => CloseScreen());
		EventManager.Instance.AddListener<FinishedRoomTransition>(OnRoomTransitionFinished);
		EventManager.Instance.AddListener<DialogueKnotCompletedEvent>(OnDialogueKnotCompleted);
	}
}
