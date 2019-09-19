using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class MediumButtonManager : Manager<MediumButtonManager>
{
    private const int MAX_AMOUNT_OF_ACTIVE_BUTTONS = 8;
    private const string WON_MINIGAME_MESSAGE = "Congratulations you won the mini game.\n" +
        "Click on the medium you want to send to the white room.\n Then click confirm.";

    public bool MinigameIsWon { get; private set; } = false;
    [HideInInspector] public int activeButtons = 0;
    [HideInInspector] public Sprite selectedMediumSprite = null;
    [HideInInspector] public List<GameObject> Puzzles = new List<GameObject>();
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        foreach(Button button in GetComponentsInChildren<Button>())
        {
            buttons.Add(button);
            button.interactable = false;
        }

        EnableNextButton();
    }

    public void EnableNextButton()
    {
        if(activeButtons >= MAX_AMOUNT_OF_ACTIVE_BUTTONS)
        {
            MinigameIsWon = true;
            PuzzleManager.Instance.DisplayPieceText(Color.magenta, WON_MINIGAME_MESSAGE);            
            return;
        }

        buttons[activeButtons].interactable = true;
        ++activeButtons;
    }

    public void Confirm()
    {
        if(!MinigameIsWon)
        {
            return;
        }

        NextRoomEvent nextRoomEvent = new NextRoomEvent();
        EventManager.Instance.RaiseEvent(nextRoomEvent);

        SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.Medium);
        EventManager.Instance.RaiseEvent(saveItemEvent);
    }
}
