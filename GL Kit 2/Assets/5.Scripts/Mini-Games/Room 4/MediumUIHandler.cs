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
	
	public bool MinigameIsWon { get; private set; } = false;
	public string ExtendedDescription { get; private set; }	

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
			MinigameIsWon = true;
			EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
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
	}
}
