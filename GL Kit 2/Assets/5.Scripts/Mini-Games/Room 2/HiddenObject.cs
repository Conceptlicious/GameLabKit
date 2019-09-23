using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias Bevers
//Overview: This script contains all the info of the hidden items. All of it is set in the inspector.
//This info is later read in the hiddenItemHandler;
//Usage: On every item in the "CollectedHiddenObjectBar".
//--------------------------------------------------

[RequireComponent(typeof(MonoBehaviour))]
public class HiddenObject : BetterMonoBehaviour
{
    [SerializeField] [TextArea] private string description = string.Empty;
    public string Description => description;
    private Button button = null;
    private Sprite sprite = null;
    private bool isFound = false;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
        button = GetComponent<Button>();

        button.onClick.AddListener(() => Clicked());

        button.interactable = false;
    }

    public void Found()
    {
        if (isFound)
        {
            return;
        }

        button.interactable = true;
        TextUpdater.Instance.CallUpdateTextCoroutine(name, description);
    }

    public void Clicked()
    {
        if(!HiddenObjectHandler.Instance.MinigameIsWon)
        {
            Debug.Log("Mini game is not won yet");
            return;
        }

        HiddenObjectHandler.Instance.lastSelectedObjectSprite = sprite;
    }
}
