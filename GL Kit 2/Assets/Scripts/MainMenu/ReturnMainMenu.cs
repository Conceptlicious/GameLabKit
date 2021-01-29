using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    private DialogueHandler dialogueHandler => DialogueHandler.instance;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartDialogue()
    {
        dialogueHandler.SetDialogue("Prof. Henk", "Intro");
    }
}
