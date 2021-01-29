using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameHandler : MonoBehaviour
{
    private Player player => Player.instance;
    private LabHandler labHandler => LabHandler.instance;

    private DialogueHandler dialogueHandler => DialogueHandler.instance;

    public static MinigameHandler instance;

    [SerializeField] GameObject labRoom;
    [SerializeField] GameObject computerScreen;
    [SerializeField] GameObject fireMinigame;
    [SerializeField] GameObject bordenMinigame;
    [SerializeField] GameObject foodMinigame;
    [SerializeField] GameObject cityMinigame;
    [SerializeField] GameObject memoryMinigame;
    [SerializeField] GameObject pleegouderMinigame;

    [Header("Search & Find")]
    [SerializeField] GameObject zoekMainCamera;
    [SerializeField] GameObject searchObject;
    [SerializeField] GameObject bordenObject;

    [Header("Pauze menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject teleportButton;
    [SerializeField] bool pauseMenuOpened;
    public bool gamePaused;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Handles switching to the different minigames
    /// </summary>
    /// <param name="game"></param>
    public void StartMinigame(string name)
    {
        GameObject game = null;
        switch (name)
        {
            case "blussen":
                game = fireMinigame;
                break;
            case "food":
                game = foodMinigame;
                break;
            case "marketing":
                game = memoryMinigame;
                break;
            case "veiligheid":
                game = bordenMinigame;
                bordenObject.SetActive(false);
                searchObject.SetActive(true);
                zoekMainCamera.SetActive(true);
                break;
            case "lokaal":
                game = cityMinigame;
                break;
            case "jeugdzorg":
                game = pleegouderMinigame;
                break;
        }
        game.SetActive(true);
        labRoom.SetActive(false);
    }

    /// <summary>
    /// Handles returning to the lab
    /// </summary>
    public void ReturnToLab(bool returnToScreen = false)
    {
        labRoom.SetActive(true);
        if (player.Inventory.Count >= 5) labHandler.FinishedMinigames();
        else if (returnToScreen) computerScreen.SetActive(true);
        fireMinigame.SetActive(false);
        bordenMinigame.SetActive(false);
        foodMinigame.SetActive(false);
        cityMinigame.SetActive(false);
        memoryMinigame.SetActive(false);
        pleegouderMinigame.SetActive(false);
    }

    /// <summary>
    /// Handles returning to the computer screen using the settings
    /// </summary>
    public void ReturnToComputerScreen()
    {
        pauseMenuOpened = !pauseMenuOpened;
        gamePaused = pauseMenuOpened;
        pauseMenu.SetActive(false);
        dialogueHandler.Close();
        ReturnToLab(true);
    }

    public void OpenPauzeMenu(bool hideTeleporter = false)
    {
        pauseMenuOpened = !pauseMenuOpened;
        gamePaused = pauseMenuOpened;
        if (hideTeleporter) teleportButton.SetActive(false);
        else teleportButton.SetActive(true);
        pauseMenu.SetActive(pauseMenuOpened);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
