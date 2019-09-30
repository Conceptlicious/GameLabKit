using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class MediumButtonManager : Manager<MediumButtonManager>
{
    private const int MAX_AMOUNT_OF_ACTIVE_BUTTONS = 8;
    private const string WON_MINIGAME_MESSAGE = "<color=#00ff00>Congratulations you won the mini game.</color>" +
        "Click on the medium you want to send to the white room.";

    public bool MinigameIsWon { get; private set; } = false;
    [HideInInspector] public int activeButtons = 0;
    [HideInInspector] public List<GameObject> Puzzles = new List<GameObject>();
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            buttons.Add(button);
            button.interactable = false;
        }

        EnableNextButton();
    }

    public void EnableNextButton()
    {
        if (activeButtons >= MAX_AMOUNT_OF_ACTIVE_BUTTONS)
        {
            MinigameIsWon = true;
            PuzzleManager.Instance.DisplayPieceText(WON_MINIGAME_MESSAGE);
            return;
        }

        buttons[activeButtons].interactable = true;
        ++activeButtons;
    }    
}
