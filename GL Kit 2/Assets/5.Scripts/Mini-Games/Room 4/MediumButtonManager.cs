using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class MediumButtonManager : Manager<MediumButtonManager>
{
    [HideInInspector] public int activeButtons = 0;
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
        buttons[activeButtons].interactable = true;
        ++activeButtons;
    }
}
