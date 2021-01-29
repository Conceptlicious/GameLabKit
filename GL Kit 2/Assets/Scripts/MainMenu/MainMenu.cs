using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private LabHandler labHandler => LabHandler.instance;

    [SerializeField] GameObject labRoom;

    private void Update()
    {
        //Handles clicking the game objects
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickedObject = Utility.GetClickedGameObject();
            if (clickedObject != null && clickedObject.name.Equals("Trophy Case"))
                SkipMinigames();
        }
    }

    public void StartGame()
    {
        StartGame(false);
    }

    public void SkipMinigames()
    {
        StartGame(true);
    }

    public void StartGame(bool skip = false)
    {
        gameObject.SetActive(false);
        labRoom.SetActive(true);
        labHandler.StartGame(skip);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
