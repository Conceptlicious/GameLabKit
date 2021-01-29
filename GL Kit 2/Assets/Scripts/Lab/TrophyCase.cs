using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyCase : MonoBehaviour
{

    DialogueHandler dialogueHandler => DialogueHandler.instance;
    LabHandler labHandler => LabHandler.instance;

    [Header("Information")]
    [SerializeField] GameObject targetObject;
    [SerializeField] GameObject screwDriverObject;
    [SerializeField] GameObject n64ControllerObject;
    [SerializeField] GameObject paintbrushObject;
    [SerializeField] GameObject filmTicketObject;

    [Header("Exit the game popup")]
    [SerializeField] bool canInteract = true;
    [SerializeField] GameObject exitGameObject;

    void Update()
    {
        if (!labHandler.CurrentState.Equals(LabHandler.State.PLACED_TROPHIES)) return;

        //Handles clicking the game objects
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickedObject = Utility.GetClickedGameObject();
            if (clickedObject != null)
            {
                //Debug.LogError($"clickedObject.name: {clickedObject.name}");
                if (!canInteract) return;

                switch (clickedObject.name)
                {
                    case "Target":
                        canInteract = false;
                        targetObject.SetActive(true);
                        break;
                    case "Screwdriver":
                        canInteract = false;
                        screwDriverObject.SetActive(true);
                        break;
                    case "n64-controller":
                        canInteract = false;
                        n64ControllerObject.SetActive(true);
                        break;
                    case "Paintbrush":
                        canInteract = false;
                        paintbrushObject.SetActive(true);
                        break;
                    case "Filmticket":
                        canInteract = false;
                        filmTicketObject.SetActive(true);
                        break;
                }
            }
        }
    }

    public void OpenExit()
    {
        canInteract = false;
        exitGameObject.SetActive(true);
    }

    public void CloseExit(bool openPrefence)
    {
        if(!openPrefence)
            canInteract = true;
        exitGameObject.SetActive(false);
        targetObject.SetActive(false);
        screwDriverObject.SetActive(false);
        n64ControllerObject.SetActive(false);
        paintbrushObject.SetActive(false);
        filmTicketObject.SetActive(false);
    }

    public void ExitGame()
    {
        labHandler.CurrentState = LabHandler.State.EXIT_GAME;
        CloseExit(false);
        dialogueHandler.SetDialogue("Prof. Henk", "Exit");
    }
}
