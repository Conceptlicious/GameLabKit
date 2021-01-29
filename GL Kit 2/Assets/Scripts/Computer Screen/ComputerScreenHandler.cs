using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerScreenHandler : MonoBehaviour
{
    public DialogueHandler dialogueHandler => DialogueHandler.instance;
    public MinigameHandler minigameHandler => MinigameHandler.instance;
    public LabHandler labHandler => LabHandler.instance;


    public void StartGezondEten(Button button)
    {
        gameObject.SetActive(false);
        labHandler.CurrentState = LabHandler.State.FOOD_MINIGAME_START;
        dialogueHandler.SetDialogue("Prof. Henk", "Clicked food");
    }

    public IEnumerator NextDialogue(string fileName, string name, float waitTime = 0.1f)
    {
        yield return new WaitForSeconds(waitTime);
        dialogueHandler.SetDialogue(fileName, name);
    }

    public void StartJeugdzorg(Button button)
    {
        //button.interactable = false;
        gameObject.SetActive(false);
        labHandler.CurrentState = LabHandler.State.JEUGDZORG;
        labHandler.TogglePortal();
    }

    public void StartGamelabMarketing(Button button)
    {
        //button.interactable = false;
        gameObject.SetActive(false);
        labHandler.CurrentState = LabHandler.State.MEMEORY_MINIGAME;
        labHandler.TogglePortal();
    }

    public void StartBorden(Button button)
    {
        //button.interactable = false;
        gameObject.SetActive(false);
        labHandler.CurrentState = LabHandler.State.VEILIGHEID_MINIGAME;
        labHandler.TogglePortal();
    }

    public void StartLokaalShoppen(Button button)
    {
        //button.interactable = false;
        gameObject.SetActive(false);
        labHandler.CurrentState = LabHandler.State.LOKAAL_SHOPPEN;
        labHandler.TogglePortal();
    }

}
