using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class MediumButtonManager : Manager<MediumButtonManager>
{
    private const int MAX_AMOUNT_OF_ACTIVE_BUTTONS = 8;

    [HideInInspector] public int activeButtons = 0;
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
        Debug.Log($"Activebuttons: {activeButtons}, MaxButtons: 8");
        if(activeButtons >= MAX_AMOUNT_OF_ACTIVE_BUTTONS)
        {
            // Won mini game method.

            Debug.Log("Mini game has been completed");
            return;
        }

        buttons[activeButtons].interactable = true;
        ++activeButtons;
    }
}
